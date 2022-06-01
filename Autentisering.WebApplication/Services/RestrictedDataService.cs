using Autentisering.Shared;
using Autentisering.WebApplication.ExternalApi;
using Refit;

namespace Autentisering.WebApplication.Services
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



        public async Task<RestrictedData> GetRestrictedData()
        {     

            try
            {
                var response = await this.restrictedDataApi.GetRestrictedData();
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
