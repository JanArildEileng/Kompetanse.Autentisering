using Autentisering.RefitApi.Services;

namespace Autentisering.WebApplication.Services
{
    public class TokenFreshService
    {
        private readonly TokenManger tokenManger;
        private readonly IIdentityAndAccessApiService identityService;

        public TokenFreshService(TokenManger tokenManger, IIdentityAndAccessApiService identityService)
        {
            this.tokenManger = tokenManger;
            this.identityService = identityService;
        }

        public async Task<(bool, string)> RefreshToken(string name)
        {
            (string accessToken, string refreshToken) = await tokenManger.GetToken(name);

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

            tokenManger.SetToken(name, getTokenResponse.AccessToken, getTokenResponse.RefreshToken);

            return (true, $" {name} successful Refresh Expire={getTokenResponse.Expire} ");
        }

    }
}
