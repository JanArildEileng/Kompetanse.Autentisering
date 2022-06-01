namespace Autentisering.WebApplication
{
  
    public class AuthentTokenCache
    {
        static public string accessToken { get; set; }

        static public Task<string> AuthorizationHeaderValueGetter()
        {
            return Task.FromResult(accessToken);
        }
    }
}
