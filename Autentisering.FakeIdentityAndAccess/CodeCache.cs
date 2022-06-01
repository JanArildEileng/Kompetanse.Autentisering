
using Microsoft.Extensions.Caching.Memory;


namespace Autentisering.FakeIdentityAndAccess;

public class CodeCache
{
    public MemoryCache Cache { get; set; }

    public CodeCache()
    {
        Cache = new MemoryCache(new MemoryCacheOptions
        {
        });
    }
}
