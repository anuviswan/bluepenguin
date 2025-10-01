using BP.PriceTracker.Services.Interfaces;
using System.Collections.Concurrent;

namespace BP.PriceTracker.Services.Services;

public class NavigationCacheService : INavigationCacheService
{
    private readonly ConcurrentDictionary<string, object> _cache = new();
    public bool Add<T>(string key, T value)
    {
        return _cache.TryAdd(key, value!);
    }

    public void Clear()
    {
        _cache.Clear();
    }

    public T? Get<T>(string key)
    {
        return _cache.TryGetValue(key, out var value) ? (T?)value : default;
    }

    public bool Remove(string key)
    {
        return _cache.TryRemove(key, out _);
    }
}
