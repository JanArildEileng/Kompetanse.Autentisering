using Autentisering.Shared.Dto.IdentityAndAccess;

namespace Autentisering.FakeIdentityAndAccess.Services.AuthorizationCode
{
    public class AuthorizationCodeContent
    {
        public string AuthorizationCode { get; internal set; }
        public string Client_id { get; internal set; }
        public User User { get; internal set; }

    }
}
