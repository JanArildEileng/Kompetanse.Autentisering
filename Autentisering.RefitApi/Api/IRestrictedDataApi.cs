using Refit;

namespace Autentisering.RefitApi.Api;

[Headers("Authorization: Bearer")]
public interface IRestrictedDataApi
{
    [Get("/api/RestrictedData")]
    Task<HttpResponseMessage> GetRestrictedData();
}
