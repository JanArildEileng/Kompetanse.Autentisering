using Refit;

namespace Autentisering.WebApplication.ExternalApi;

public interface IIdentityApi
{
     [Get("/Identity")]
    Task<HttpResponseMessage> GetIdentityHttpResponseMessage(string userName, string password);

    [Get("/Authorization/Code")]
    Task<HttpResponseMessage> GetAuthorizationCode(string client_id,string userName, string password);


    [Get("/Token/IdToken")]
    Task<HttpResponseMessage> GetIdToken(string authorizationCode);

    [Get("/Token/AccessToken")]
    Task<HttpResponseMessage> GetAccessToken(string authorizationCode);

    [Get("/Userinfo")]
    Task<HttpResponseMessage> GetUserinfo(string AccessToken);
   

}
