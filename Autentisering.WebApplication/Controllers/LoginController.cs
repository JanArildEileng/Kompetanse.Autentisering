using Autentisering.RefitApi.Services;
using Autentisering.WebApplication.AppServices.Features.IdentityAndAccess;
using Autentisering.WebApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult> Login([FromServices] LoginService loginService , string userName = "TestUSer", string password = "TestUSer")
    {

        (bool status, ClaimsPrincipal? claimsPrincipal, string text) = await loginService.Login(userName, password);

        if (!status)
            return BadRequest(text);


        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.Now.AddDays(1),
            IsPersistent = true,
        };

        await HttpContext.SignInAsync(claimsPrincipal!, authProperties);

        return Ok(text);
    }

    [Authorize]
    [HttpPost("Logout", Name = "Logout")]
    public async Task<ActionResult> Logout()
    {
        var name = GetIdentityName();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok($"{name} successful Logout");
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