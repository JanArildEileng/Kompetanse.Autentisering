using Autentisering.RefitApi;
using Autentisering.Shared;
using Autentisering.WebApplication.AppServices.Contracts;
using Microsoft.Extensions.Logging;
using Refit;
using System.Net.Http.Json;

namespace Autentisering.WebApplication.Infrastructure
{
    public class BackendApiService : IBackendApiService
    {
        private readonly IBackendApi backendApi;
        private readonly ILogger<BackendApiService> logger;

        public BackendApiService(ILogger<BackendApiService> logger, IBackendApi backendApi)
        {
            this.logger = logger;
            this.backendApi = backendApi;
        }



        public async Task<RestrictedData> GetRestrictedData(string AccessToken)
        {

            try
            {
                var response = await backendApi.GetRestrictedData(AccessToken);
                if (response.IsSuccessStatusCode)
                {

                    var restrictedData = await response.Content.ReadFromJsonAsync<RestrictedData>();
                    return restrictedData;
                }
                throw new Exception("GetRestrictedData failed " + response.ReasonPhrase);
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

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastHttpResponseMessage()
        {
            try
            {
                var response = await backendApi.GetWeatherForecastHttpResponseMessage();
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
