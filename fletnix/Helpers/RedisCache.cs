using System;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;

namespace fletnix.Helpers
{
    public class RedisCache : IRedisCache
    {
        private IDistributedCache _cache;

        public RedisCache(IDistributedCache cache)
        {
            _cache = cache;
        }
        
        public bool Exists(string key)
        {
            return _cache.Get(key) != null;
        }

        public string Retrieve(string key)
        {
            return Encoding.UTF8.GetString(_cache.Get(key));
        }

        public async void Add(string key, string value)
        {
            var options = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = new TimeSpan(0,12,0,0)
            };
            await _cache.SetAsync(key, Encoding.UTF8.GetBytes(value), options);
        }

        public async void Remove(string key)
        {
           await _cache.RemoveAsync(key);
        }
    }
}