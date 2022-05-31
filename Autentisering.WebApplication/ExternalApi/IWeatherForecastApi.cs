using Refit;

namespace Autentisering.WebApplication.ExternalApi;

[Headers("Authorization: Bearer")]
public interface IWeatherForecastApi
{
     [Get("api/WeatherForecast")]
    Task<HttpResponseMessage> GetWeatherForecastHttpResponseMessage();
}
