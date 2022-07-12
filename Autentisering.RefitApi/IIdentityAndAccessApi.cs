using Refit;

namespace Authorization.RefitApi;

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

    [Get("/oauth/token")]
    Task<HttpResponseMessage> GetTokenCodeFlow(string grant_type = "authorization_code",
                                        string code = "B6E4-B7B966BA3A89",
                                        string redirect_uri = "",
                                        string client_id = "45663491-1F66-4447-B6E4-B7B966BA3A89",
                                        string client_secret = "secret_123");
}
