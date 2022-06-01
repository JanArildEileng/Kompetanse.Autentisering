
using Microsoft.Extensions.Caching.Memory;


namespace Autentisering.FakeIdentityAndAccess;

public class AuthorizationCodeCache
{
    private MemoryCache cache { get; set; }
    private MemoryCacheEntryOptions cacheEntryOptions;

    public AuthorizationCodeCache()
    {
        cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5)); 

        cache = new MemoryCache(new MemoryCacheOptions
        {
        });
    }

    public void Set(string authorizationCode, AuthorizationCodeContent authorizationCodeContent)
    {
        cache.Set(authorizationCode, authorizationCodeContent, this.cacheEntryOptions);
    }

    public bool TryGet(string authorizationCode, out AuthorizationCodeContent authorizationCodeContent)
    {
        return cache.TryGetValue(authorizationCode, out authorizationCodeContent);
    }



}
