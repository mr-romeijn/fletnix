using System;
using System.Collections.Generic;

namespace fletnix.Helpers
{
    public class MemoryCache : IRedisCache
    {
        private static Dictionary<String, String> _Cache = new Dictionary<string, string>();

        public void Add(String key, String value)
        {
            _Cache.Add(key,value);
        }

        public void Remove(string key)
        {
            _Cache.Remove(key);
        }

        public bool Exists(string key)
        {
            return _Cache.ContainsKey(key);
        }
        
        public string Retrieve(String key)
        {
            return _Cache[key];
        }

    }
}