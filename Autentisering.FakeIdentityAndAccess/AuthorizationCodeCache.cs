
using Microsoft.Extensions.Caching.Memory;


namespace Autentisering.FakeIdentityAndAccess;

public class AuthorizationCodeCache
{
    private MemoryCache Cache { get; set; }
    private MemoryCacheEntryOptions cacheEntryOptions;

    public AuthorizationCodeCache()
    {
        cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5)); 

        Cache = new MemoryCache(new MemoryCacheOptions
        {
        });
    }

    public void Set(string authorizationCode, AuthorizationCodeContent authorizationCodeContent)
    {
        Cache.Set(authorizationCode, authorizationCodeContent, this.cacheEntryOptions);
    }

    public bool TryGet(string authorizationCode, out AuthorizationCodeContent authorizationCodeContent)
    {
        return Cache.TryGetValue(authorizationCode, out authorizationCodeContent);
    }



}
