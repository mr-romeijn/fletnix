using System;
using System.Collections.Generic;

namespace fletnix.Helpers
{
    public static class MemoryCache
    {
        private static Dictionary<String, String> _Cache = new Dictionary<string, string>();

        public static void Add(String key, String value)
        {
            _Cache.Add(key,value);
        }

        public static bool Exists(string key)
        {
            return _Cache.ContainsKey(key);
        }
        
        public static string Retrieve(String key)
        {
            return _Cache[key];
        }

    }
}