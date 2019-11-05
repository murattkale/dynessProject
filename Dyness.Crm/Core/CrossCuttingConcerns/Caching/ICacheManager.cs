namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        void Add(string key, object data, int cacheTime = 60);
        bool Contains(string key);
        void Delete(string key);
        void Clear();
        void DeleteByPattern(string pattern);
    }
}
