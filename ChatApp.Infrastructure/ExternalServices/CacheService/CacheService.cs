using ChatApp.Application.Interfaces.ExternalService;
using Microsoft.Extensions.Caching.Memory;

namespace ChatApp.Infrastructure.ExternalServices.CacheService
{
    internal class CacheService(IMemoryCache memoryCache) : ICacheService<string>
    {
        public async Task<string> Get(string key)
        {
            if (memoryCache.TryGetValue(key, out object? result))
            {
                if (result == null) return "";
                return await Task.FromResult((string)result);
            }
            return "";
        }

        public async Task Set(string key, string value, TimeSpan? expiration = null)
        {
            if (expiration != null)
            {
                memoryCache.Set(key, value,(TimeSpan) expiration);
            }
            else
            {
                memoryCache.Set(key, value);
            }
            await Task.CompletedTask;
        }

        public async Task Remove(string key)
        {
            memoryCache.Remove(key);
            await Task.CompletedTask;
        }
    }
}
