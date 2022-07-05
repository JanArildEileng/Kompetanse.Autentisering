using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorization.FakeIdentityAndAccess.Pages
{
    public class AuthorizeApplicationModel : PageModel
    {
        public IHttpClientFactory httpClientFactory1 { get; set; }
        public AuthorizeApplicationModel(IHttpClientFactory httpClientFactory)
        {
            httpClientFactory1 = httpClientFactory;

        }
        public void OnGet()
        {
        }

       public async  void  OnPostOK(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://api.github.com/repos/dotnet/AspNetCore.Docs/branches");
            

            var client = httpClientFactory1.CreateClient();

            var response = await client.SendAsync(request);

            
        }
    }
}
