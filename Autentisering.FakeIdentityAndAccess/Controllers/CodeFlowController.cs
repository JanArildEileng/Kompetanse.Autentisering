using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.FakeIdentityAndAccess.Controllers
{
    
    [ApiController]
    public class CodeFlowController : ControllerBase
    {
        [Route("oauth/authorize")]   
        [HttpGet]
        public RedirectResult InitControlFlow(string response_type = "code", string client_id = "45663491-1F66-4447-B6E4-B7B966BA3A89", string scope="photo", string redirect_url = "http://localhost:7072/")
        {
            string returnValue = "hello";
            return Redirect($"https://localhost:7134/AuthorizeApplication?redirect_url={redirect_url}"); 
        }

        [Route("oauth/token")]
        [HttpGet]
        public string Token(string grant_type = "authorization_code",
                                                    string code = "B6E4-B7B966BA3A89",
                                                    string redirect_uri = "",
                                                    string client_id = "45663491-1F66-4447-B6E4-B7B966BA3A89",
                                                    string client_secret = "secret_123")
        {
            string returnValue = "hello";
            return returnValue;
        }
    }
}
