using Refit;

namespace Autentisering.RefitApi.Api;

public interface IIdentityApi
{
    [Get("/Identity")]
    Task<HttpResponseMessage> GetIdentityHttpResponseMessage(string userName, string password);

    [Get("/Authorization/Code")]
    Task<HttpResponseMessage> GetAuthorizationCode(string client_id, string userName, string password);


    [Get("/Token")]
    Task<HttpResponseMessage> GetToken(string authorizationCode);


    [Get("/Token/Refresh")]
    Task<HttpResponseMessage> GetRefreshedTokens(string refreshToken);



    [Get("/Userinfo")]
    Task<HttpResponseMessage> GetUserinfo(string AccessToken);


}
