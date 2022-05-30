using Refit;

namespace Autentisering.WebApplication.IdentityAndAccess;

public interface IdentityApi
{
     [Get("/Identity")]
    Task<HttpResponseMessage> GetIdentityHttpResponseMessage(string userName, string password);
}
