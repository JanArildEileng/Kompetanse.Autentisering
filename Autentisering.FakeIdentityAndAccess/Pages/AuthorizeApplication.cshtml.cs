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

       public RedirectResult OnPostOK(string redirect_url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            redirect_url);

            var dictionary = QueryHelpers.ParseQuery(redirect_url);

            var client = httpClientFactory1.CreateClient();
            var url = dictionary["redirect_url"];
            return Redirect(url + "CodeFlow?Autorization_code=B6E4-B7B966BA3A89");
            //var response = await client.SendAsync(request);

           //var httpRequestMessage = new HttpRequestMessage(
           //HttpMethod.Get,
           //url + "CodeFlow?Autorization_code=B6E4-B7B966BA3A89")
           // {            
           // };

           // var httpClient = httpClientFactory1.CreateClient();
           // var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

           // if (httpResponseMessage.IsSuccessStatusCode)
           // {
           //     using var contentStream =
           //         await httpResponseMessage.Content.ReadAsStreamAsync();

               
           // }


        }
    }
}
