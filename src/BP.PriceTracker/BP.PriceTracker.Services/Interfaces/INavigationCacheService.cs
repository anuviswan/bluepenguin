namespace BP.PriceTracker.Services.Interfaces;

public interface INavigationCacheService
{
    public bool Add<T>(string key, T value);
    public T? Get<T>(string key);
    public bool Remove(string key);
    public void Clear();
}
