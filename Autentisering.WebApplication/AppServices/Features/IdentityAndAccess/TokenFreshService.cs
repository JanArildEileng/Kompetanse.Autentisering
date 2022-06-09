using Autentisering.WebApplication.AppServices.Contracts;
using Autentisering.WebApplication.Services;

namespace Autentisering.WebApplication.AppServices.Features.IdentityAndAccess
{
    public class TokenFreshService
    {
        private readonly TokenCacheManager tokenCacheManager;
        private readonly IIdentityAndAccessApiService identityService;

        public TokenFreshService(TokenCacheManager tokenCacheManager, IIdentityAndAccessApiService identityService)
        {
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

    }
}
