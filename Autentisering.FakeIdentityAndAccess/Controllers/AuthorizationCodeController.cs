using Microsoft.AspNetCore.Mvc;

namespace Autentisering.FakeIdentityAndAccess.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationCodeController : ControllerBase
    {
        private readonly ILogger<AuthorizationCodeController> _logger;

        public AuthorizationCodeCache authorizationCodeCache { get; }

        public AuthorizationCodeController(ILogger<AuthorizationCodeController> logger, [FromServices] AuthorizationCodeCache authorizationCodeCache)
        {
            _logger = logger;
            this.authorizationCodeCache = authorizationCodeCache;
        }

        //return authorization_code
        [HttpGet("", Name = "GetAuthorizationCode")]
        public string GetAuthorizationCode([FromServices] UserRepoitory userRepoitory,  string client_id="1234", string userName="test", string password="test")
        {
            string authorizationCode = String.Empty;

            //sjekk user/pass, return valid authorization_code if ok
            var user = userRepoitory.GetUser(userName);

            if (user!=null)
            {
                authorizationCode = Guid.NewGuid().ToString();
                //må lagre denn i MemoryCach
                AuthorizationCodeContent authorizationCodeContent = new AuthorizationCodeContent()
                {
                    AuthorizationCode = authorizationCode,
                    Client_id = client_id,
                    User= user
                };

                authorizationCodeCache.Set(authorizationCode, authorizationCodeContent);
            }
            return authorizationCode;
        }
    }
}