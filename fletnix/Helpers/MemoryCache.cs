using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fletnix.Helpers
{
    public class MemoryCache : ICache
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
        
        public  Task<string> Retrieve(string key)
        {
            return Task.Run(() => _Cache[key]);
        }

    }
}