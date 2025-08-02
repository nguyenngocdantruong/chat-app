using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Shared.Services;
using Microsoft.Extensions.Caching.Memory;

namespace ChatApp.Infrastructure.ExternalServices.CacheService
{
    internal class CacheService(IMemoryCache memoryCache) : ICacheService<string>
    {
        public string Get(string key)
        {
            object? item = memoryCache.Get(key);
            if (item == null) return "";
            return item.ToString() ?? "";
        }

        public void Set(string key, string value, TimeSpan? expiration = null)
        {
            if (expiration != null)
            {
                memoryCache.Set(key, value,(TimeSpan) expiration);
            }
            else
            {
                memoryCache.Set(key, value);
            }
        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }
    }
}
