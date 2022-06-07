using Refit;

namespace Autentisering.RefitApi.Api;

public interface IRestrictedDataApi
{
    [Get("/api/RestrictedData")]
    Task<HttpResponseMessage> GetRestrictedData([Authorize("Bearer")] string accessToken);
}
