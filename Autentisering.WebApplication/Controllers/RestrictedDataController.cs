using Authorization.Shared.Dto.BackEnd;
using Authorization.WebBFFApplication.AppServices.Features.Backend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Authorization.WebBFFApplication.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RestrictedDataController : ControllerBase
{
    private readonly ILogger<RestrictedDataController> _logger;

    public RestrictedDataController(ILogger<RestrictedDataController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetRestrictedData")]
    public async Task<ActionResult<RestrictedData>> GetRestrictedData([FromServices] RestrictedDataService restrictedDataService)
    {
        var identity = HttpContext.User.Identities.First();
        var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).First().Value;

        (bool status, RestrictedData restrictedData, string text) = await restrictedDataService.GetRestrictedData(name);

        if (!status)
        {
            return BadRequest(text);
        }

        return Ok(restrictedData);
    }
}