using Autentisering.Shared.IdentityAndAccess;

namespace Autentisering.RefitApi.Services
{
    public interface IIdentityService
    {
        Task<string> Login(string userName = "TestUSer", string password = "Password");

        Task<string> GetAuthorizationCode(string client_id, string userName, string password);

  
        Task<GetTokenResponse> GetToken(string authorizationCode);
        Task<GetTokenResponse> GetRefreshedTokens(string refreshToken);


        Task<string> GetUserinfo(string AccessToken);

    }
}