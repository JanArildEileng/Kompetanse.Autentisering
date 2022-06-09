using Autentisering.RefitApi.Services;
using Autentisering.Shared;
using Autentisering.WebApplication.Services;

namespace Autentisering.WebApplication.AppServices.Backend
{
    public class RestrictedDataService
    {
        private readonly ILogger<RestrictedDataService> logger;
        private readonly IBackendApiService backendApiService;
        private readonly TokenCacheManager tokenCacheManager;

        public RestrictedDataService(ILogger<RestrictedDataService> logger, IBackendApiService backendApiService, TokenCacheManager tokenCacheManager)
        {
            this.logger = logger;
            this.backendApiService = backendApiService;
            this.tokenCacheManager = tokenCacheManager;
        }

        public async Task<(bool, RestrictedData?, string)> GetRestrictedData(string name)
        {
            (string accessToken, _) = await tokenCacheManager.GetToken(name);

            if (string.IsNullOrEmpty(accessToken))
            {
                return (false, null, $"Mangler accesstoken");
            }

            logger.LogInformation("GetRestrictedData accessToken={accessToken}", accessToken);

            RestrictedData restrictedData = await backendApiService.GetRestrictedData(accessToken);
            return (true, restrictedData, "");
        }



    }


}



