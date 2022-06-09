using Autentisering.Shared.Dto.IdentityAndAccess;

namespace Autentisering.FakeIdentityAndAccess
{



    public class UserRepoitory
    {
        static List<User> users = new List<User>()
        {
            new User()
            {
                Name="Jan",
                Guid=new Guid("0a8d5bb6-3a1c-4ef6-9822-7d20dd7ea287"),
                Role=Roletype.Super
            },
            new User()
            {
                Name="Gordon",
                Guid=new Guid("1a789704-00a2-4bc5-b38e-314c7404ee08"),
                Role=Roletype.Cat
            },


        };


        public User GetUser(string name)
        {
            return users.Where(e => e.Name == name).FirstOrDefault();
        }

        public User GetUser(Guid guid)
        {
            return users.Where(e => e.Guid == guid).FirstOrDefault();
        }

        internal List<User> GetAllUser()
        {
            return users;
        }
    }
}
