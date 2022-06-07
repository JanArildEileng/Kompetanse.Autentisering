using Autentisering.FakeIdentityAndAccess.TokenGenerators;
using Autentisering.Shared.IdentityAndAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Autentisering.FakeIdentityAndAccess.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
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


    
       


        [HttpGet("/Token", Name = "GetToken")]
        public GetTokenResponse GetToken(string authorizationCode, [FromServices] AccessTokenGenerator accessTokenGenerator, [FromServices] IdTokenGenerator idTokenGenerator)
        {
            GetTokenResponse getTokenResponse = new(); 

            string accessToken = String.Empty;

            if (authorizationCodeCache.TryGet(authorizationCode, out AuthorizationCodeContent authorizationCodeContent))
            {
                //ok...sjekk på client_id?
                  //Generer AccessToken..
                 getTokenResponse.AccessToken = accessTokenGenerator.GetAccessToken();

                //Generer IdToken..
                 getTokenResponse.IdToken = idTokenGenerator.GetIdToken();


                getTokenResponse.Expire = DateTime.Now.AddMinutes(5);
            }
            else
            {


            }

            return getTokenResponse;
        }



        [HttpGet("/Userinfo", Name = "GetUserinfo")]
        public string GetUserinfo(string access_token)
        {
            string userinfo = String.Empty;
            return userinfo;
        }






    }
}