using Autentisering.Shared.Dto.IdentityAndAccess;

namespace Autentisering.FakeIdentityAndAccess.AppServices.Contracts
{
    public interface IUserRepoitory
    {
        User GetUser(Guid guid);
        User GetUser(string name);
        List<User> GetAllUser();
    }
}