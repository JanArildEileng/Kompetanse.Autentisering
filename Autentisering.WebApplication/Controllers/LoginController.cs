
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
    public IIdentityService identityService { get; }

    public LoginController(ILogger<LoginController> logger, IIdentityService identityService)
    {
        _logger = logger;
      
        this.identityService = identityService;
    }



    [HttpPost(Name = "Login")]
    public async Task<ActionResult> Login([FromServices] AuthorizationCodeManger authorizationCodeManger , [FromServices] TokenValidetorService tokenValidetorService,string userName="TestUSer",string password= "TestUSer")
    {
        string authorizationCode = await identityService.GetAuthorizationCode("1234", userName, password);

        if (String.IsNullOrEmpty(authorizationCode))
        {
            return BadRequest($" Login {userName} not successful login (authorizationCode)");
        }


        var idToken = await identityService.GetIdToken(authorizationCode);


        if (String.IsNullOrEmpty(idToken))
        {
            return BadRequest($" Login {userName} not successful login (idToken)");
        }
        //legg p� validering her..!
  
        JwtSecurityToken jwtSecurityToken = tokenValidetorService.ReadValidateIdToken(idToken);

        if (jwtSecurityToken==null)
        {
            return BadRequest($"Login {userName} not successful invalid jwtSecurityToken");
        }

        //hent ut info fra claims i JWT (idtoken)
        var claims = jwtSecurityToken.Claims.ToList();
        var name = claims.Where(e => e.Type == ClaimTypes.Name).Select(e => e.Value).FirstOrDefault();

        //bygg opp ClaimsPrincipal for Cookie

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

        identity.AddClaim(new Claim(ClaimTypes.Name, name));
        identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
   
        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.Now.AddDays(1),
            IsPersistent = true,
        };

        //lagre authorizationCode i cache
        authorizationCodeManger.SetAuthorizationCode(name, authorizationCode);

        await HttpContext.SignInAsync( new ClaimsPrincipal(principal), authProperties);

        return Ok($"{name} successful login: Idtoken={idToken}");
    }

    [Microsoft.AspNetCore.Authorization.Authorize]
    [HttpPost("Logout",Name = "Logout")]
    public async Task<ActionResult> Logout()
    {
        var identity = this.HttpContext.User.Identities.First();
        var name= identity.Claims.Where(c => c.Type == ClaimTypes.Name).First().Value;

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok($" {name} successful Logout");
    }


}