using Autentisering.RefitApi.Services;
using Autentisering.Shared;
using Autentisering.WebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Autentisering.WebApplication.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RestrictedDataController : ControllerBase
{

    private readonly ILogger<RestrictedDataController> _logger;
    private readonly IRestrictedDataService restrictedDataService;
    private readonly IIdentityAndAccessApiService identityService;

    public RestrictedDataController(ILogger<RestrictedDataController> logger, IRestrictedDataService restrictedDataService, TokenCacheManager accessTokenManger)
    {
        _logger = logger;
        this.restrictedDataService = restrictedDataService;
        this.identityService = identityService;
    }


   
    [HttpGet(Name = "GetRestrictedData")]
    public async Task<ActionResult<RestrictedData>> GetRestrictedData([FromServices]TokenCacheManager tokenCacheManager)
    {
        var identity = this.HttpContext.User.Identities.First();
        var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).First().Value;

   
        (string accessToken,_) = await tokenCacheManager.GetToken(name);

        if ( string.IsNullOrEmpty(accessToken))
        {
            return BadRequest("Mangler accesstoken");
        }
        
        _logger.LogInformation("GetRestrictedData accessToken={accessToken}", accessToken);
    
        RestrictedData restrictedData = await this.restrictedDataService.GetRestrictedData(accessToken);
        return Ok(restrictedData);
    }
}