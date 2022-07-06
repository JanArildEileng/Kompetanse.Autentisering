using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorization.WebBFFApplication.Pages
{
    public class StartLogoncshtmlModel : PageModel
    {

        public IHttpClientFactory httpClientFactory1 { get; set; }

        public StartLogoncshtmlModel(IHttpClientFactory httpClientFactory)
        {
            httpClientFactory1 = httpClientFactory;

        }

        public void OnGet()
        {
        }

        public  RedirectResult OnPostLogon()
        {
            //var request = new HttpRequestMessage(HttpMethod.Get,
            //"https://localhost:7134/oauth/authorize?response_type=code&client_id=45663491-1F66-4447-B6E4-B7B966BA3A89&scope=photo");


            //var client = httpClientFactory1.CreateClient();

            //var response = await client.SendAsync(request);

            return Redirect("https://localhost:7134/oauth/authorize?response_type=code&client_id=45663491-1F66-4447-B6E4-B7B966BA3A89&scope=photo&redirect_url=http://localhost:7072/");


        }
    }
}
