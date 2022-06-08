
using Autentisering.RefitApi.Services;
using Autentisering.WebApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Autentisering.WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "Login")]
    public async Task<ActionResult> Login([FromServices] IIdentityAndAccessApiService identityService,[FromServices] TokenManger tokenManger, [FromServices] TokenValidetorService tokenValidetorService, string userName = "TestUSer", string password = "TestUSer")
    {
        string authorizationCode = await identityService.GetAuthorizationCode("1234", userName, password);

        if (String.IsNullOrEmpty(authorizationCode))
        {
            return BadRequest($" Login {userName} not successful login (authorizationCode)");
        }

        var getTokenResponse = await identityService.GetToken(authorizationCode);


        if (String.IsNullOrEmpty(getTokenResponse.IdToken))
        {
            return BadRequest($" Login {userName} not successful login (idToken)");
        }

        JwtSecurityToken jwtSecurityToken = tokenValidetorService.ReadValidateIdToken(getTokenResponse.IdToken);

        if (jwtSecurityToken == null)
        {
            return BadRequest($"Login {userName} not successful invalid jwtSecurityToken");
        }

        //hent ut info fra claims i JWT (idtoken)
        var claims = jwtSecurityToken.Claims.ToList();
        var name = claims.Where(e => e.Type == ClaimTypes.Name).Select(e => e.Value).FirstOrDefault();
        var role = claims.Where(e => e.Type == ClaimTypes.Role).Select(e => e.Value).FirstOrDefault();

        //bygg opp ClaimsPrincipal for Cookie

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

        identity.AddClaim(new Claim(ClaimTypes.Name, name));
        identity.AddClaim(new Claim(ClaimTypes.Role, role));

        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.Now.AddDays(1),
            IsPersistent = true,
        };

   
        if (!String.IsNullOrEmpty(getTokenResponse.AccessToken))
        {
            tokenManger.SetToken(name, getTokenResponse.AccessToken, getTokenResponse.RefreshToken);
        }

        await HttpContext.SignInAsync(new ClaimsPrincipal(principal), authProperties);

        var jti = claims.Where(e => e.Type == JwtRegisteredClaimNames.Jti).Select(e => e.Value).FirstOrDefault();

        return Ok($"{name} ({role})  successful login: jti={jti}");
    }

    [Authorize]
    [HttpPost("Logout", Name = "Logout")]
    public async Task<ActionResult> Logout()
    {

        var name = GetIdentityName();

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok($" {name}successful Logout");
    }

    [Authorize]
    [HttpPost("Refresh", Name = "Refresh")]
    public async Task<ActionResult> Refresh([FromServices] TokenFreshService tokenFreshService)
    {
 
        (bool status, string text) = await tokenFreshService.RefreshToken(GetIdentityName());

        switch (status)
        {
            case true: return Ok(text);
            case false: return BadRequest(text);
        }
    }



    private string GetIdentityName()
    {
        var identity = this.HttpContext.User.Identities.First();
        var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).First().Value;
        return name;
    }


}