using Microsoft.Extensions.Caching.Memory;

namespace Autentisering.WebApplication.Services
{
    public class AuthorizationCodeManger
    {
        string key = @"{userName}:authorizationCode";
        private readonly ILogger<AuthorizationCodeManger> logger;
        private readonly IMemoryCache memoryCache;
        MemoryCacheEntryOptions memoryCacheEntryOptions;


        public AuthorizationCodeManger(ILogger<AuthorizationCodeManger> logger,IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
            memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5));

        }


        public void SetAuthorizationCode(string userName, string authorizationCode)
        {
            memoryCache.Set(key,authorizationCode, memoryCacheEntryOptions);
        }


        public async Task<string> GetAuthorizationCode(string userName)
        {
            string authorizationCode;

            if (memoryCache.TryGetValue(key, out authorizationCode))
            {
                return authorizationCode;
            }

           


            return authorizationCode;
        }


    }
}
