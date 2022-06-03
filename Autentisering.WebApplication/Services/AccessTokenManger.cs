using Microsoft.Extensions.Caching.Memory;

namespace Autentisering.WebApplication.Services
{
    public class AccessTokenManger
    {
        const string TokenKey = "AccessToken";
        private readonly ILogger<AccessTokenManger> logger;
        private readonly IMemoryCache memoryCache;
        private readonly IIdentityService identityService;
        MemoryCacheEntryOptions memoryCacheEntryOptions;


        public AccessTokenManger(ILogger<AccessTokenManger> logger,IMemoryCache memoryCache, IIdentityService identityService)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
            this.identityService = identityService;
            memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5));

        }


        public async Task<string> GetAccessToken(string authorizationCode)
        {
            string accessToken;

            if (memoryCache.TryGetValue(TokenKey, out accessToken))
            {
                return accessToken;
            }

            accessToken = await identityService.GetAccessToken(authorizationCode);
            if (accessToken != null)
            {
                logger.LogInformation("Add token to memoryCache");
                memoryCache.Set(TokenKey, accessToken, memoryCacheEntryOptions);
            }


            return accessToken;
        }


    }
}
