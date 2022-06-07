using Autentisering.RefitApi.Services;
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


        private  string Key(string username) => $"{TokenKey}:{username}";

        public AccessTokenManger(ILogger<AccessTokenManger> logger,IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
            memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5));

        }

        public void SetAccessToken(string username,string accessToken)
        {
  
              if (accessToken != null)
            {
                logger.LogInformation("Add token to memoryCache");
                memoryCache.Set(Key, accessToken, memoryCacheEntryOptions);
            }
        }

        public async Task<string> GetAccessToken(string username)
        {
      
            if (memoryCache.TryGetValue(Key, out string accessToken))
            {
                return accessToken;
            }

           //fresh ?


            return null;
        }


    }
}
