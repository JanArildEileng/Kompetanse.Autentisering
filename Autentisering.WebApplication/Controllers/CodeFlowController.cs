using Authorization.WebBFFApplication.AppServices.Features.IdentityAndAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Authorization.WebBFFApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class CodeFlowController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;

    public CodeFlowController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    [HttpPost()]
    public async Task<ActionResult> GetTokenCodeFlow(string Autorization_code)
    {
        var getTokenResponse = await identityService.GetToken(authorizationCode);

        if (string.IsNullOrEmpty(getTokenResponse.IdToken))
        {
            return (false, null, $" Login {userName} not successful login (idToken)");
        }

        JwtSecurityToken jwtSecurityToken = tokenValidetorService.ReadValidateIdToken(getTokenResponse.IdToken);

        if (jwtSecurityToken == null)
        {
            return (false, null, $"Login {userName} not successful invalid jwtSecurityToken");
        }

        //hent ut info fra claims i JWT (idtoken)
        var claims = jwtSecurityToken.Claims.ToList();
        var name = claims.Where(e => e.Type == ClaimTypes.Name).Select(e => e.Value).FirstOrDefault();
        var role = claims.Where(e => e.Type == ClaimTypes.Role).Select(e => e.Value).FirstOrDefault();
        var jti = claims.Where(e => e.Type == JwtRegisteredClaimNames.Jti).Select(e => e.Value).FirstOrDefault();
    }


}