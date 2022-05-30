using Autentisering.Shared;
using Autentisering.WebApplication.Backend;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Autentisering.WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastApi weatherForecastApi;



    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastApi weatherForecastApi)
    {
        _logger = logger;
        this.weatherForecastApi = weatherForecastApi;
    }

    [HttpGet(Name = "GetWeatherForecastHttpResponseMessage")]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecastHttpResponseMessage()
    {
        try
        {
            var response = await this.weatherForecastApi.GetWeatherForecastHttpResponseMessage();
            if (response.IsSuccessStatusCode)
            {

                var weather = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
                return Ok(weather);
            }
            return BadRequest(response.ReasonPhrase);
        }
        catch (ApiException apiException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }
}