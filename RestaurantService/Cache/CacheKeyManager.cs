using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Cache
{
    public class CacheKeyManager
    {
        private readonly ConcurrentDictionary<string, bool> _keys = new();
        public void AddKey(string key) => _keys[key] = true;
        public void RemoveKey(string key) => _keys.TryRemove(key, out _);
        public IEnumerable<string> GetKeysByPrefix(string prefix) => 
        _keys.Keys.Where(k => k.StartsWith(prefix));
    }
}