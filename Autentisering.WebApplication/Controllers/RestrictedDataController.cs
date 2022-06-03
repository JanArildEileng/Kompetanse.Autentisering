using Autentisering.Shared;
using Autentisering.WebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Autentisering.WebApplication.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RestrictedDataController : ControllerBase
{

    private readonly ILogger<RestrictedDataController> _logger;
    private readonly IRestrictedDataService restrictedDataService;
    private readonly IIdentityService identityService;

    public RestrictedDataController(ILogger<RestrictedDataController> logger, IRestrictedDataService restrictedDataService, AccessTokenManger accessTokenManger)
    {
        _logger = logger;
        this.restrictedDataService = restrictedDataService;
        this.identityService = identityService;
    }


   
    [HttpGet(Name = "GetRestrictedData")]
    public async Task<ActionResult<RestrictedData>> GetRestrictedData([FromServices]AccessTokenManger accessTokenManger)
    {
        var identity = this.HttpContext.User.Identities.First();
        var authorizationCode = identity.Claims.Where(c => c.Type == "authorizationCode").First().Value;

        _logger.LogInformation("GetRestrictedData authorizationCode={authorizationCode}", authorizationCode);




//        var accessToken = await identityService.GetAccessToken(authorizationCode);
        var accessToken = await accessTokenManger.GetAccessToken(authorizationCode);



        if ( string.IsNullOrEmpty(accessToken))
        {
            return BadRequest("Mangler accesstoken");
        }
        
        _logger.LogInformation("GetRestrictedData accessToken={accessToken}", accessToken);

        AuthentTokenCache.accessToken = accessToken;



        RestrictedData restrictedData = await this.restrictedDataService.GetRestrictedData();
        return Ok(restrictedData);
       
    }
}