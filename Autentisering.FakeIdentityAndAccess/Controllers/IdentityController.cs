using Autentisering.FakeIdentityAndAccess.TokenGenerators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Autentisering.FakeIdentityAndAccess.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<IdentityController> _logger;

        public AuthorizationCodeCache authorizationCodeCache { get; }

        public IdentityController(ILogger<IdentityController> logger, AuthorizationCodeCache authorizationCodeCache)
        {
            _logger = logger;
            this.authorizationCodeCache = authorizationCodeCache;

        }

        [HttpGet(Name = "GetIdentity")]
        public string GetIdentity(string userName,string password)
        {
            return "Jan";
        }


        //return authorization_code
        [HttpGet("/Authorization/Code", Name = "GetAuthorizationCode")]
        public string GetAuthorizationCode(string client_id="1234", string userName="test", string password="test")
        {
            string authorizationCode = String.Empty;

            //sjekk user/pass, return valid authorization_code if ok
            
            if (userName.Equals(password))
            {
                authorizationCode = Guid.NewGuid().ToString();
                //må lagre denn i MemoryCach
                AuthorizationCodeContent authorizationCodeContent = new AuthorizationCodeContent()
                {
                    AuthorizationCode = authorizationCode,
                    Client_id = client_id
                };

                authorizationCodeCache.Set(authorizationCode, authorizationCodeContent);
            }
                
            
            return authorizationCode;
        }


        [HttpGet("/Token/IdToken", Name = "GetIdToken")]
        public string GetIdToken(string authorizationCode, [FromServices] IdTokenGenerator idTokenGenerator)
        {
             string idtoken=String.Empty;

            if (authorizationCodeCache.TryGet(authorizationCode, out AuthorizationCodeContent authorizationCodeContent))
            {
                //ok...sjekk på client_id?

                //Genererte IDToken..
                idtoken = idTokenGenerator.GetIdToken();


            }


                return idtoken;
        }

        [HttpGet("/Token/AccessToken", Name = "GetAccessToken")]
        public string GetAccessToken(string authorizationCode, [FromServices] AccessTokenGenerator accessTokenGenerator)
        {
            string accessToken = String.Empty;

            if (authorizationCodeCache.TryGet(authorizationCode, out AuthorizationCodeContent authorizationCodeContent))
            {
                //ok...sjekk på client_id?

                //Genererte AccessToken..
                accessToken = accessTokenGenerator.GetAccessToken();
            } else
            {


            }



            return accessToken;
        }

        [HttpGet("/Userinfo", Name = "GetUserinfo")]
        public string GetUserinfo(string access_token)
        {
            string userinfo = String.Empty;
            return userinfo;
        }






    }
}