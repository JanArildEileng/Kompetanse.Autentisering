using Autentisering.Shared;

namespace Autentisering.WebApplication.AppServices.Contracts
{
    public interface IBackendApiService
    {
        Task<RestrictedData> GetRestrictedData(string accessToken);

        Task<IEnumerable<WeatherForecast>> GetWeatherForecastHttpResponseMessage();
    }
}