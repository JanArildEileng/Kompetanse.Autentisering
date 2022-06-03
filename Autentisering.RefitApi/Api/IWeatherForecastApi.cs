using Refit;

namespace Autentisering.RefitApi.Api;

[Headers("Authorization: Bearer")]
public interface IWeatherForecastApi
{
    [Get("/api/WeatherForecast")]
    Task<HttpResponseMessage> GetWeatherForecastHttpResponseMessage();
}
