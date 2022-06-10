using Authorization.Shared.Dto.IdentityAndAccess;

namespace Authorization.FakeIdentityAndAccess.AppServices.Contracts
{
    public interface IUserRepoitory
    {
        User GetUser(Guid guid);
        User GetUser(string name);
        List<User> GetAllUser();
    }
}