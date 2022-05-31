using Autentisering.Shared;
using Autentisering.WebApplication.Services;
using Microsoft.AspNetCore.Mvc;


namespace Autentisering.WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class RestrictedDataController : ControllerBase
{

    private readonly ILogger<RestrictedDataController> _logger;
    private readonly IRestrictedDataService restrictedDataService;



    public RestrictedDataController(ILogger<RestrictedDataController> logger, IRestrictedDataService restrictedDataService)
    {
        _logger = logger;
        this.restrictedDataService = restrictedDataService;
    }

    [HttpGet(Name = "GetRestrictedData")]
    public async Task<ActionResult<RestrictedData>> GetRestrictedData()
    {

        RestrictedData restrictedData = await this.restrictedDataService.GetRestrictedData();
        return Ok(restrictedData);
       
    }
}