
using Autentisering.WebApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult> Login(string userName="TestUSer",string password="Password")
    {
        var identityName= await identityService.Login(userName, password);


        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identityName));
        identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.Now.AddDays(1),
            IsPersistent = true,
        };

        await HttpContext.SignInAsync( new ClaimsPrincipal(principal), authProperties);


        return Ok($" {identityName} successful login");
    }

    [Microsoft.AspNetCore.Authorization.Authorize]
    [HttpPost("Logout",Name = "Logout")]
    public async Task<ActionResult> Logout()
    {
        var identity = this.HttpContext.User.Identities.First();
        var name= identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok($" {name} successful Logout");
    }


}