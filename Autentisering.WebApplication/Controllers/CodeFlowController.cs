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
    public async Task<ActionResult> GetToken(string Autorization_code)
    {
        //Hent token
    }


}