using Microsoft.Extensions.Caching.Memory;

namespace Authorization.WebBFFApplication.AppServices.Services
{
    public class TokenCacheManager
    {
        private readonly ILogger<TokenCacheManager> logger;
        private readonly IMemoryCache memoryCache;
  
        private string AccessKey(string username) => $"AccessToken:{username}";
        private string RefreshKey(string username) => $"RefreshToken:{username}";

        public TokenCacheManager(ILogger<TokenCacheManager> logger, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
         
        }

        public void SetToken(string username, string accessToken, string refreshToken)
        {
            if (accessToken != null)
            {
                logger.LogInformation("Add token to memoryCache");
                memoryCache.Set(AccessKey, accessToken, TimeSpan.FromMinutes(5));
            }

            if (refreshToken != null)
            {
                logger.LogInformation("Add refreshToken to memoryCache");
                memoryCache.Set(RefreshKey, refreshToken, TimeSpan.FromMinutes(5));
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
