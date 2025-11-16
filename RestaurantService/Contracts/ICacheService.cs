using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Contracts
{
    public interface ICacheService
    {
        Task<T> CachingAsync<T>(string key, Func<Task<T>> factory, TimeSpan timeSpan);
        Task RemoveCacheAsync(string key);
        Task RemoveByPrefixAsync(string[] prefix);
    }
}