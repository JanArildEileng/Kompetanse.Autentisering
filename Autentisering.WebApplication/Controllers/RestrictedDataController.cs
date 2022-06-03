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
    private readonly IIdentityService identityService;

    public RestrictedDataController(ILogger<RestrictedDataController> logger, IRestrictedDataService restrictedDataService, AccessTokenManger accessTokenManger)
    {
        _logger = logger;
        this.restrictedDataService = restrictedDataService;
        this.identityService = identityService;
    }


   
    [HttpGet(Name = "GetRestrictedData")]
    public async Task<ActionResult<RestrictedData>> GetRestrictedData([FromServices] AuthorizationCodeManger authorizationCodeManger,[FromServices]AccessTokenManger accessTokenManger)
    {
        var identity = this.HttpContext.User.Identities.First();
        var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).First().Value;



        var authorizationCode=await authorizationCodeManger.GetAuthorizationCode(name);


        _logger.LogInformation("GetRestrictedData authorizationCode={authorizationCode}  user={name}", authorizationCode,name);




//        var accessToken = await identityService.GetAccessToken(authorizationCode);
        var accessToken = await accessTokenManger.GetAccessToken(authorizationCode);



        if ( string.IsNullOrEmpty(accessToken))
        {
            return BadRequest("Mangler accesstoken");
        }
        
        _logger.LogInformation("GetRestrictedData accessToken={accessToken}", accessToken);

        AuthentTokenCache.SetBearerToken(accessToken);



        RestrictedData restrictedData = await this.restrictedDataService.GetRestrictedData();
        return Ok(restrictedData);
       
    }
}