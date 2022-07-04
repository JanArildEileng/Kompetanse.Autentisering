using Microsoft.Extensions.Caching.Memory;

namespace Authorization.WebBFFApplication.AppServices.Services
{
    public class TokenCacheManager
    {
        private readonly ILogger<TokenCacheManager> logger;
        private readonly IMemoryCache memoryCache;
 
        public TokenCacheManager(ILogger<TokenCacheManager> logger, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
         
        }

        public void SetToken(string username, string accessToken, string refreshToken)
        {
            string AccessKey = $"AccessToken:{username}";
            string RefreshKey = $"RefreshToken:{username}";

            logger.LogInformation("Set Token for {username}  AccessKey={AccessKey}  RefreshKey={RefreshKey}", username, AccessKey, RefreshKey);


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
            string AccessKey = $"AccessToken:{username}";
            string RefreshKey= $"RefreshToken:{username}";


            string accessToken;
            string refreshToken;


            logger.LogInformation("Get Token for {username}  AccessKey={AccessKey}  RefreshKey={RefreshKey}", username, AccessKey, RefreshKey);


            memoryCache.TryGetValue(AccessKey, out accessToken);
            memoryCache.TryGetValue(RefreshKey, out refreshToken);

            return (accessToken, refreshToken);
        }


    }
}
