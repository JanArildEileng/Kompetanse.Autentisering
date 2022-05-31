using Autentisering.Shared;

namespace Autentisering.WebApplication.Services
{
    public interface IRestrictedDataService
    {
        Task<RestrictedData> GetRestrictedData();
    }
}