using System;

namespace fletnix.Helpers
{
    public interface IRedisCache
    {
        bool Exists(string key);
        string Retrieve(String key);
        void Add(String key, String value);
        void Remove(String key);
    }
}