using Autentisering.Shared;

namespace Autentisering.RefitApi.Services
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecastHttpResponseMessage();
    }
}