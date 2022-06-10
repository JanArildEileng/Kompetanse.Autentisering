using Microsoft.Extensions.Caching.Memory;

namespace Autentisering.FakeIdentityAndAccess.Services.AuthorizationCode;

public class AuthorizationCodeCache
{
    private IMemoryCache memoryCache { get; set; }
    private MemoryCacheEntryOptions cacheEntryOptions;

    public AuthorizationCodeCache(IMemoryCache memoryCache)
    {
        cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5));
        this.memoryCache = memoryCache;
    }

    public void Set(string authorizationCode, AuthorizationCodeContent authorizationCodeContent)
    {
        memoryCache.Set(authorizationCode, authorizationCodeContent, cacheEntryOptions);
    }

    public bool TryGet(string authorizationCode, out AuthorizationCodeContent authorizationCodeContent)
    {
        return memoryCache.TryGetValue(authorizationCode, out authorizationCodeContent);
    }
}
