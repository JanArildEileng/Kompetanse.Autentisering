namespace Autentisering.FakeIdentityAndAccess
{

    public class User
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
    }

    public class UserRepoitory
    {
        static List<User> users = new List<User>()
        {
            new User()
            {
                Name="Jan",
                Guid=new Guid("0a8d5bb6-3a1c-4ef6-9822-7d20dd7ea287")
            }

        };


        public User GetUser(string name)
        {
            return users.Where(e => e.Name == name).FirstOrDefault();
        }

        public User GetUser(Guid guid)
        {
            return users.Where(e => e.Guid == guid).FirstOrDefault();
        }
    }
}
