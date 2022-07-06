using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Authorization.FakeIdentityAndAccess.Pages
{
    public class AuthorizeApplicationModel : PageModel
    {
        public IHttpClientFactory httpClientFactory1 { get; set; }
        public string Redirect_url { get; set; }

        public AuthorizeApplicationModel(IHttpClientFactory httpClientFactory)
        {
            httpClientFactory1 = httpClientFactory;

        }
        public void OnGet()
        {
        }

       public  RedirectResult OnPostOK(string redirect_url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            redirect_url);

            var dictionary = QueryHelpers.ParseQuery(redirect_url);

            var client = httpClientFactory1.CreateClient();

            return Redirect("http://www.vg.no");
            //var response = await client.SendAsync(request);


        }
    }
}
