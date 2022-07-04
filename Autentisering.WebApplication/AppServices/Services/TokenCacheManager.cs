using Microsoft.Extensions.Caching.Memory;

namespace Authorization.WebBFFApplication.AppServices.Services
{
    public class TokenCacheManager
    {
        private readonly ILogger<TokenCacheManager> logger;
        private readonly IMemoryCache memoryCache;
        private readonly HashSet<string> userNames=new HashSet<string>();

        public TokenCacheManager(ILogger<TokenCacheManager> logger, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
        }

        public void SetToken(string username, string accessToken, string refreshToken)
        {
            string AccessKey = $"AccessToken:{username}";
            string RefreshKey = $"RefreshToken:{username}";

            logger.LogDebug("Set Token for {username}  AccessKey={AccessKey}  RefreshKey={RefreshKey}", username, AccessKey, RefreshKey);

            if (accessToken != null)
            {
                logger.LogDebug("Add token to memoryCache");
                memoryCache.Set(AccessKey, accessToken, TimeSpan.FromMinutes(5));
            }

            if (refreshToken != null)
            {
                userNames.Add(username);
                logger.LogDebug("Add refreshToken to memoryCache");
                memoryCache.Set(RefreshKey, refreshToken, TimeSpan.FromMinutes(5));
            }
        }

        public async Task<(string, string)> GetToken(string username)
        {
            string AccessKey = $"AccessToken:{username}";
            string RefreshKey= $"RefreshToken:{username}";

            string accessToken;
            string refreshToken;

            logger.LogDebug("Get Token for {username}  AccessKey={AccessKey}  RefreshKey={RefreshKey}", username, AccessKey, RefreshKey);

            memoryCache.TryGetValue(AccessKey, out accessToken);
            memoryCache.TryGetValue(RefreshKey, out refreshToken);

            return (accessToken, refreshToken);
        }



        public void RemoveToken(string username)
        {
            string AccessKey = $"AccessToken:{username}";
            string RefreshKey = $"RefreshToken:{username}";

            string accessToken;
            string refreshToken;

            logger.LogDebug("Remove Token for {username}  AccessKey={AccessKey}  RefreshKey={RefreshKey}", username, AccessKey, RefreshKey);

            memoryCache.Remove(AccessKey);
            memoryCache.Remove(RefreshKey);

            userNames.Remove(username);

            return;
        }



        public async Task<List<string>> GetAllUserNames()
        {
            return userNames.ToList(); ;
        }




    }
}
