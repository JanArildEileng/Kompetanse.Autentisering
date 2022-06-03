namespace Autentisering.WebApplication;

public class AuthentTokenCache
{
    static private string accessToken; 

    static public void SetBearerToken(string accessToken)
    {
        AuthentTokenCache.accessToken = accessToken;
    }

    static public Task<string> AuthorizationHeaderValueGetter()
    {
        return Task.FromResult(accessToken);
    }
}
