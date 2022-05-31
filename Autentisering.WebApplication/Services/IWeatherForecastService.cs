using Autentisering.Shared;

namespace Autentisering.WebApplication.Services
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecastHttpResponseMessage();
    }
}