using Autentisering.WebBFFApplication.AppServices.Contracts;
using Autentisering.WebBFFApplication.Services;
using Common.TokenUtils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Autentisering.WebBFFApplication.AppServices.Features.IdentityAndAccess
{
    public class LoginService
    {
        private readonly TokenCacheManager tokenCacheManager;
        private readonly IIdentityAndAccessApiService identityService;
        private readonly TokenValidetorService tokenValidetorService;

        public LoginService(TokenCacheManager tokenCacheManager, IIdentityAndAccessApiService identityService, TokenValidetorService tokenValidetorService)
        {
            this.tokenCacheManager = tokenCacheManager;
            this.identityService = identityService;
            this.tokenValidetorService = tokenValidetorService;
        }

        public async Task<(bool, ClaimsPrincipal?, string)> Login(string userName, string password)
        {
            string authorizationCode = await identityService.GetAuthorizationCode("1234", userName, password);

            if (string.IsNullOrEmpty(authorizationCode))
            {
                return (false, null, $" Login {userName} not successful login (authorizationCode)");
            }

            var getTokenResponse = await identityService.GetToken(authorizationCode);

            if (string.IsNullOrEmpty(getTokenResponse.IdToken))
            {
                return (false, null, $" Login {userName} not successful login (idToken)");
            }

            JwtSecurityToken jwtSecurityToken = tokenValidetorService.ReadValidateIdToken(getTokenResponse.IdToken);

            if (jwtSecurityToken == null)
            {
                return (false, null, $"Login {userName} not successful invalid jwtSecurityToken");
            }

            //hent ut info fra claims i JWT (idtoken)
            var claims = jwtSecurityToken.Claims.ToList();
            var name = claims.Where(e => e.Type == ClaimTypes.Name).Select(e => e.Value).FirstOrDefault();
            var role = claims.Where(e => e.Type == ClaimTypes.Role).Select(e => e.Value).FirstOrDefault();
            var jti = claims.Where(e => e.Type == JwtRegisteredClaimNames.Jti).Select(e => e.Value).FirstOrDefault();

            //bygg opp ClaimsPrincipal for Cookie
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(ClaimTypes.Name, name));
            identity.AddClaim(new Claim(ClaimTypes.Role, role));

            var principal = new ClaimsPrincipal(identity);

            if (!string.IsNullOrEmpty(getTokenResponse.AccessToken))
            {
                tokenCacheManager.SetToken(name, getTokenResponse.AccessToken, getTokenResponse.RefreshToken);
            }

            return (true, new ClaimsPrincipal(principal), $"{name} ({role})  successful login: jti={jti}");
        }


    }

}

