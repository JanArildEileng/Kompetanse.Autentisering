using Refit;

namespace Autentisering.WebApplication.ExternalApi;

[Headers("Authorization: Bearer")]
public interface IRestrictedDataApi
{
     [Get("api/RestrictedData")]
    Task<HttpResponseMessage> GetRestrictedData();
}
