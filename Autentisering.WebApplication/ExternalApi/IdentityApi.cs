using Refit;

namespace Autentisering.WebApplication.ExternalApi;

public interface IdentityApi
{
     [Get("/Identity")]
    Task<HttpResponseMessage> GetIdentityHttpResponseMessage(string userName, string password);
}
