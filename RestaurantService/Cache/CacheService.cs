using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using RestaurantService.Contracts;

namespace RestaurantService.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly CacheKeyManager _cacheKeyManager = new ();
        public CacheService(IMemoryCache cache, CacheKeyManager cacheKeyManager)
        {
            _cache = cache;
            _cacheKeyManager = cacheKeyManager;
        }
        public async Task<T> CachingAsync<T>(string key, Func<Task<T>> factory, TimeSpan timeSpan)
        {
            if(_cache.TryGetValue(key, out T cachedResult))
            return cachedResult;
            var resultFac = await factory();       
            _cache.Set(key, resultFac, TimeSpan.FromMinutes(timeSpan.Minutes));
            return resultFac;
        }

        public Task RemoveByPrefixAsync(string[] prefixs)
        {
            for(int i = 0; i < prefixs.Length; i++)
            {
                var keysToRemove = _cacheKeyManager.GetKeysByPrefix(prefixs[i]).ToList();
                foreach(var keys in keysToRemove)
                {
                    _cache.Remove(keys);
                    _cacheKeyManager.RemoveKey(keys);
                }
            }
           return Task.CompletedTask;
        }

        public async Task RemoveCacheAsync(string key)
        {
            _cache.Remove(key);
        }
    }
}