using Authorization.Shared.Dto.IdentityAndAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Authorization.FakeIdentityAndAccess.Services.AuthorizationCode;
using Authorization.FakeIdentityAndAccess.Services.TokenGenerators;

namespace Authorization.FakeIdentityAndAccess.Controllers
{
    
    [ApiController]
    public class CodeFlowController : ControllerBase
    {
        [Route("oauth/authorize")]   
        [HttpGet]
        public RedirectResult InitControlFlow(string response_type = "code", string client_id = "45663491-1F66-4447-B6E4-B7B966BA3A89", string scope="photo", string redirect_url = "https://localhost:7072/")
        {
            string returnValue = "hello";
            return Redirect($"https://localhost:7134/AuthorizeApplication?redirect_url={redirect_url}"); 
        }

        [Route("oauth/token")]
        [HttpGet]

        public GetTokenResponse Token(  [FromServices] AccessTokenGenerator accessTokenGenerator, 
                                        [FromServices] IdTokenGenerator idTokenGenerator, 
                                        [FromServices] RefreshTokenGenerator refreshTokenGenerator,
                                        string grant_type = "authorization_code",
                                        string code = "B6E4-B7B966BA3A89",
                                        string redirect_uri = "",
                                        string client_id = "45663491-1F66-4447-B6E4-B7B966BA3A89",
                                        string client_secret = "secret_123")
        {
            GetTokenResponse getTokenResponse = new();

            string accessToken = string.Empty;

            //if (authorizationCodeCache.TryGet(authorizationCode, out AuthorizationCodeContent authorizationCodeContent))
            // check authorization code
            if(true)
            {
                AuthorizationCodeContent authorizationCodeContent = new()
                {
                    AuthorizationCode = code,
                    Client_id = client_id,
                    User = new()
                    {
                        Guid = Guid.NewGuid(),
                        Name = "Svein",
                        Role = Roletype.Basic
                    }
                };
                //ok...sjekk på client_id?
                //Generer AccessToken..
                User user = authorizationCodeContent.User;

                getTokenResponse.AccessToken = accessTokenGenerator.GetAccessToken(user);

                //Generer IdToken..
                getTokenResponse.IdToken = idTokenGenerator.GetIdToken(user);
                //Generer FreshToken
                getTokenResponse.RefreshToken = refreshTokenGenerator.GetFreshToken(user);


                getTokenResponse.Expire = DateTime.Now.AddMinutes(5);
            }
            else
            {
            }

            return getTokenResponse;
        }
    }
}
