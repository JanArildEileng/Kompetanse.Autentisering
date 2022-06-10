using Microsoft.Extensions.Caching.Memory;

namespace Autentisering.WebBFFApplication.Services
{
    public class TokenCacheManager
    {
        private readonly ILogger<TokenCacheManager> logger;
        private readonly IMemoryCache memoryCache;
        MemoryCacheEntryOptions memoryCacheEntryOptions;

        private string AccessKey(string username) => $"AccessToken:{username}";
        private string RefreshKey(string username) => $"RefreshToken:{username}";

        public TokenCacheManager(ILogger<TokenCacheManager> logger, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
            memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5));
        }

        public void SetToken(string username, string accessToken, string refreshToken)
        {
            if (accessToken != null)
            {
                logger.LogInformation("Add token to memoryCache");
                memoryCache.Set(AccessKey, accessToken, memoryCacheEntryOptions);
            }

            if (refreshToken != null)
            {
                logger.LogInformation("Add refreshToken to memoryCache");
                memoryCache.Set(RefreshKey, refreshToken, memoryCacheEntryOptions);
            }
        }

        public async Task<(string, string)> GetToken(string username)
        {
            string accessToken;
            string refreshToken;
            memoryCache.TryGetValue(AccessKey, out accessToken);
            memoryCache.TryGetValue(RefreshKey, out refreshToken);
       
            return (accessToken, refreshToken);
        }


    }
}
