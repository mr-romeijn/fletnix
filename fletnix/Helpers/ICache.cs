using System;
using System.Threading.Tasks;

namespace fletnix.Helpers
{
    public interface ICache
    {
        bool Exists(string key);
        Task<string> Retrieve(string key);
        void Add(String key, String value);
        void Remove(String key);
    }
}