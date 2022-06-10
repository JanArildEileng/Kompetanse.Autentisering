using Authorization.Shared.Dto.BackEnd;

namespace Authorization.WebBFFApplication.AppServices.Contracts
{
    public interface IBackendApiService
    {
        Task<RestrictedData> GetRestrictedData(string accessToken);

        Task<IEnumerable<WeatherForecast>> GetWeatherForecastHttpResponseMessage();
    }
}