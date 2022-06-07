using Autentisering.Shared;

namespace Autentisering.RefitApi.Services
{
    public interface IRestrictedDataService
    {
        Task<RestrictedData> GetRestrictedData(string accessToken);
    }
}