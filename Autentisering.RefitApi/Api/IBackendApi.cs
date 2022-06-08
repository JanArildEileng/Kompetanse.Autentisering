using Refit;

namespace Autentisering.RefitApi.Api;

public interface IBackendApi
{
    [Get("/api/RestrictedData")]
    Task<HttpResponseMessage> GetRestrictedData([Authorize("Bearer")] string accessToken);

    [Get("/api/WeatherForecast")]
    Task<HttpResponseMessage> GetWeatherForecastHttpResponseMessage();
}
