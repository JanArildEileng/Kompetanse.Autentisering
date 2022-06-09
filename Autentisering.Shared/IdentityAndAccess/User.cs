namespace Autentisering.Shared.Dto.IdentityAndAccess;
public enum Roletype
{
    Basic, Cat, Master, Super
}
public class User
{
    public string Name { get; set; }
    public Guid Guid { get; set; }
    public Roletype Role { get; set; }
}
