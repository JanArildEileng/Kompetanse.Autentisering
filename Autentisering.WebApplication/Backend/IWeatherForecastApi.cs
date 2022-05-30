using Refit;

namespace Autentisering.WebApplication.Backend;

[Headers("Authorization: Bearer")]
public interface IWeatherForecastApi
{
     [Get("api/WeatherForecast")]
    Task<HttpResponseMessage> GetWeatherForecastHttpResponseMessage();
}
