
namespace Autentisering.WebApplication.Services
{
    public interface IIdentityService
    {
        Task<string> Login(string userName = "TestUSer", string password = "Password");
    }
}