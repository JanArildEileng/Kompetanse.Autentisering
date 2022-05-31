using Autentisering.Shared;
using Autentisering.WebApplication.ExternalApi;
using Refit;

namespace Autentisering.WebApplication.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherForecastApi weatherForecastApi;
        private readonly ILogger<WeatherForecastService> logger;

        public WeatherForecastService(ILogger<WeatherForecastService> logger, IWeatherForecastApi weatherForecastApi)
        {
            this.logger = logger;
            this.weatherForecastApi = weatherForecastApi;
        }



        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastHttpResponseMessage()
        {
            try
            {
                var response = await this.weatherForecastApi.GetWeatherForecastHttpResponseMessage();
                if (response.IsSuccessStatusCode)
                {

                    var weather = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
                    return weather;
                }
                throw new Exception("WeatherForecast failed " + response.ReasonPhrase);
            }
            catch (ApiException apiException)
            {
                logger.LogError(" ApiException {Message} ", apiException.Message);

                throw;
            }
            catch (Exception exp)
            {
                logger.LogError(" Exception {Message} ", exp.Message);
                throw;
            }
        }
    }
}
