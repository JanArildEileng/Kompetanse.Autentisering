using Autentisering.Shared;
using Autentisering.WebApplication.Backend;
using Autentisering.WebApplication.IdentityAndAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Security.Claims;

namespace Autentisering.WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{

    private readonly ILogger<LoginController> _logger;
    private readonly IdentityApi identityApi;

    public LoginController(ILogger<LoginController> logger, IdentityApi identityApi)
    {
        _logger = logger;
        this.identityApi = identityApi;
    }

    [HttpPost(Name = "Login")]
    public async Task<ActionResult> Login(string userName="TestUSer",string password="Password")
    {
        var identityName="";

        try
        {
            var response = await this.identityApi.GetIdentityHttpResponseMessage(userName, password);
            if (response.IsSuccessStatusCode)
            {
                identityName = await response.Content.ReadAsStringAsync();
            } else
                return BadRequest(response.ReasonPhrase);
        }
        catch (ApiException apiException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }


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