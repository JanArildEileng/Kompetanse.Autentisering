using Refit;

namespace Autentisering.RefitApi.Api;

public interface IIdentityAndAccessApi
{
    [Get("/AuthorizationCode")]
    Task<HttpResponseMessage> GetAuthorizationCode(string client_id, string userName, string password);

    [Get("/JwtToken")]
    Task<HttpResponseMessage> GetToken(string authorizationCode);

    [Get("/JwtToken/Refresh")]
    Task<HttpResponseMessage> GetRefreshedTokens(string refreshToken);

    [Get("/Identity")]
    Task<HttpResponseMessage> GetIdentityHttpResponseMessage(string userName, string password);

    [Get("/Userinfo")]
    Task<HttpResponseMessage> GetUserinfo(string AccessToken);
}
