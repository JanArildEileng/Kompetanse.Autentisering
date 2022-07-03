using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.FakeIdentityAndAccess.Controllers
{
    [Route("oauth")]
    [ApiController]
    public class ControlFlowController : ControllerBase
    {
        [HttpGet(Name = "authorize")]
        public RedirectResult InitControlFlow(string response_type = "code", string client_id = "45663491-1F66-4447-B6E4-B7B966BA3A89", string scope="photo")
        {
            string returnValue = "hello";
            return Redirect("http://www.google.com"); 
        }
    }
}
