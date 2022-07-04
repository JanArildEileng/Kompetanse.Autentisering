using Authorization.WebBFFApplication.AppServices.Contracts;
using Authorization.WebBFFApplication.AppServices.Services;

namespace Authorization.WebBFFApplication.AppServices.Features.IdentityAndAccess
{
    public class TokenFreshService
    {
        private readonly ILogger<TokenFreshService> logger;
        private readonly TokenCacheManager tokenCacheManager;
        private readonly IIdentityAndAccessApiService identityService;

        public TokenFreshService(ILogger<TokenFreshService> logger,TokenCacheManager tokenCacheManager, IIdentityAndAccessApiService identityService)
        {
            this.logger = logger;
            this.tokenCacheManager = tokenCacheManager;
            this.identityService = identityService;
        }

        public async Task<(bool, string)> RefreshToken(string name)
        {
            (string accessToken, string refreshToken) = await tokenCacheManager.GetToken(name);

            if (string.IsNullOrEmpty(accessToken))
            {
                return (false, "Mangler accesstoken");
            }
            if (string.IsNullOrEmpty(refreshToken))
            {
                return (false, "Mangler refreshToken");
            }

            var getTokenResponse = await identityService.GetRefreshedTokens(refreshToken);


            if (getTokenResponse == null)
            {
                return (false, $" {name} unsuccessful Refresh");
            }

            tokenCacheManager.SetToken(name, getTokenResponse.AccessToken, getTokenResponse.RefreshToken);

            return (true, $" {name} successful Refresh Expire={getTokenResponse.Expire} ");
        }

        public async Task<(bool success, string messsage)> RefreshToken()
        {

            List<string> userNames= await tokenCacheManager.GetAllUserNames();



            foreach (var userName in userNames)
            {
                (bool success,string message)= await RefreshToken(userName);
                logger.LogInformation("RefreshToken {message}", message);
            }

            return (true, "");

          
        }
    }
}
