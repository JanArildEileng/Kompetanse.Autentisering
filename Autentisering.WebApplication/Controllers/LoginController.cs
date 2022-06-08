
using Autentisering.RefitApi.Services;
using Autentisering.WebApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    public IIdentityAndAccessApiService identityService { get; }

    public LoginController(ILogger<LoginController> logger, IIdentityAndAccessApiService identityService)
    {
        _logger = logger;
      
        this.identityService = identityService;
    }



    [HttpPost(Name = "Login")]
    public async Task<ActionResult> Login([FromServices] TokenManger tokenManger, [FromServices] TokenValidetorService tokenValidetorService,string userName="TestUSer",string password= "TestUSer")
    {
        string authorizationCode = await identityService.GetAuthorizationCode("1234", userName, password);

        if (String.IsNullOrEmpty(authorizationCode))
        {
            return BadRequest($" Login {userName} not successful login (authorizationCode)");
        }

        var getTokenResponse = await identityService.GetToken(authorizationCode);


        var idToken = getTokenResponse.IdToken;




        if (String.IsNullOrEmpty(idToken))
        {
            return BadRequest($" Login {userName} not successful login (idToken)");
        }
        //legg på validering her..!
  
        JwtSecurityToken jwtSecurityToken = tokenValidetorService.ReadValidateIdToken(idToken);

        if (jwtSecurityToken==null)
        {
            return BadRequest($"Login {userName} not successful invalid jwtSecurityToken");
        }

        //hent ut info fra claims i JWT (idtoken)
        var claims = jwtSecurityToken.Claims.ToList();
        var name = claims.Where(e => e.Type == ClaimTypes.Name).Select(e => e.Value).FirstOrDefault();
         var role= claims.Where(e => e.Type == ClaimTypes.Role).Select(e => e.Value).FirstOrDefault();

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


        var accessToken = getTokenResponse.AccessToken;


        if (!String.IsNullOrEmpty(accessToken))
        {
            tokenManger.SetToken(name,accessToken, getTokenResponse.RefreshToken);
        }
       
        

        await HttpContext.SignInAsync( new ClaimsPrincipal(principal), authProperties);

        var jti = claims.Where(e => e.Type == JwtRegisteredClaimNames.Jti).Select(e => e.Value).FirstOrDefault();



        return Ok($"{name} ({role})  successful login: jti={jti}");
    }

    [Microsoft.AspNetCore.Authorization.Authorize]
    [HttpPost("Logout",Name = "Logout")]
    public async Task<ActionResult> Logout()
    {
      
        var name = GetIdentityName();

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok($" {name}successful Logout");
    }


    [HttpPost("Refresh", Name = "Refresh")]
    public async Task<ActionResult> Refresh([FromServices] TokenManger tokenManger)
    {
        var name = GetIdentityName();

        (string accessToken,string refreshToken) = await tokenManger.GetToken(name);

        if (string.IsNullOrEmpty(accessToken))
        {
            return BadRequest("Mangler accesstoken");
        }
        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest("Mangler refreshToken");
        }

        var getTokenResponse = await identityService.GetRefreshedTokens(refreshToken);

        if (getTokenResponse != null)
        {
            tokenManger.SetToken(name, getTokenResponse.AccessToken, getTokenResponse.RefreshToken);
            return Ok($" {name}successful Refresh Expire={getTokenResponse.Expire} ");
        }

        return BadRequest($" {name} unsuccessful Refresh");
    }




    private string GetIdentityName()
    {
        var identity = this.HttpContext.User.Identities.First();
        var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).First().Value;
        return name;
    }


}