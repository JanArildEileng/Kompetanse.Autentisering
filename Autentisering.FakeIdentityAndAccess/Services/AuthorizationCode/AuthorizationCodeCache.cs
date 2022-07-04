using Microsoft.Extensions.Caching.Memory;

namespace Authorization.FakeIdentityAndAccess.Services.AuthorizationCode;

public class AuthorizationCodeCache
{
    private IMemoryCache memoryCache { get; set; }

    public AuthorizationCodeCache(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }

    public void Set(string authorizationCode, AuthorizationCodeContent authorizationCodeContent)
    {
        memoryCache.Set(authorizationCode, authorizationCodeContent, TimeSpan.FromMinutes(5));
    }

    public bool TryGet(string authorizationCode, out AuthorizationCodeContent authorizationCodeContent)
    {
        return memoryCache.TryGetValue(authorizationCode, out authorizationCodeContent);
    }
}
