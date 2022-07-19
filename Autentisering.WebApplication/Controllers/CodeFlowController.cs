using Authorization.WebBFFApplication.AppServices.Contracts;
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

    [HttpGet()]
    public async Task<ActionResult> GetTokenCodeFlow(string Autorization_code, [FromServices] IIdentityAndAccessApiService identityService)
    {
        var getTokenResponse = await identityService.GetToken(Autorization_code);

        if (string.IsNullOrEmpty(getTokenResponse.IdToken))
        {
            return Unauthorized();
        }

        return Ok(getTokenResponse);
    }


}