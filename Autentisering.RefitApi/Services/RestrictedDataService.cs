using Autentisering.RefitApi.Api;
using Autentisering.Shared;
using Microsoft.Extensions.Logging;
using Refit;
using System.Net.Http.Json;

namespace Autentisering.RefitApi.Services
{
    public class RestrictedDataService : IRestrictedDataService
    {
        private readonly IRestrictedDataApi restrictedDataApi;
        private readonly ILogger<RestrictedDataService> logger;

        public RestrictedDataService(ILogger<RestrictedDataService> logger, IRestrictedDataApi restrictedDataApi)
        {
            this.logger = logger;
            this.restrictedDataApi = restrictedDataApi;
        }



        public async Task<RestrictedData> GetRestrictedData(string AccessToken)
        {

            try
            {
                var response = await restrictedDataApi.GetRestrictedData(AccessToken);
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
    }
}
