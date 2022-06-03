using Autentisering.RefitApi.Services;
using Autentisering.Shared;
using Autentisering.WebApplication.Services;
using Microsoft.AspNetCore.Mvc;


namespace Autentisering.WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService weatherForecastService;



    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
    {
        _logger = logger;
        this.weatherForecastService = weatherForecastService;
    }

    [HttpGet(Name = "GetWeatherForecastHttpResponseMessage")]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecastHttpResponseMessage()
    {

        var weatherForecast = await this.weatherForecastService.GetWeatherForecastHttpResponseMessage();
        return Ok(weatherForecast);
       
    }
}