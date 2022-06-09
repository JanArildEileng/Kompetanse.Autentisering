using Autentisering.Shared;
using Autentisering.WebApplication.AppServices.Contracts;
using Autentisering.WebApplication.Services;
using Microsoft.AspNetCore.Mvc;


namespace Autentisering.WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IBackendApiService backendApiService;



    public WeatherForecastController(ILogger<WeatherForecastController> logger, IBackendApiService backendApiService)
    {
        _logger = logger;
        this.backendApiService = backendApiService;
    }

    [HttpGet(Name = "GetWeatherForecastHttpResponseMessage")]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecastHttpResponseMessage()
    {

        var weatherForecast = await this.backendApiService.GetWeatherForecastHttpResponseMessage();
        return Ok(weatherForecast);
       
    }
}