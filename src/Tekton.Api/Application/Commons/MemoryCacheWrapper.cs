using Microsoft.Extensions.Caching.Memory;

namespace Tekton.Api.Application.Commons;

public class MemoryCacheWrapper : IMemoryCacheWrapper
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheWrapper(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void Set<T>(string key, T cache, MemoryCacheEntryOptions options)
    {
        _memoryCache.Set(key, cache, options);
    }

    public bool TryGetValue<T>(string Key, out T cache)
    {
        if (_memoryCache.TryGetValue(Key, out T cachedItem))
        {
            cache = cachedItem;
            return true;
        }
        cache = default(T);
        return false;
    }
}
public interface IMemoryCacheWrapper
{
    bool TryGetValue<T>(string Key, out T cache);
    void Set<T>(string key, T cache, MemoryCacheEntryOptions options);
}