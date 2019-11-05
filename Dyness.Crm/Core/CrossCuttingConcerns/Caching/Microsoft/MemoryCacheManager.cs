using System;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        protected ObjectCache cache => MemoryCache.Default;
        public void Add(string key, object data, int cacheTime)
        {
            if (data != null)
            {
                var policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)
                };
                cache.Add(new CacheItem(key, data), policy);
            }
        }

        public void Clear()
        {
            foreach (var item in cache)
            {
                Delete(item.Key);
            }
        }

        public bool Contains(string key)
        {
            return cache.Contains(key);
        }

        public void Delete(string key)
        {
            cache.Remove(key);
        }

        public T Get<T>(string key)
        {
            return (T)cache[key];
        }

        public void DeleteByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cache.Where(x => regex.IsMatch(x.Key)).Select(x => x.Key).ToList();

            foreach (var key in keysToRemove)
                Delete(key);
        }
    }
}
