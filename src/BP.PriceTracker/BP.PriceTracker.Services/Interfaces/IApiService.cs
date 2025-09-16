namespace BP.PriceTracker.Services.Interfaces;

public interface IApiService
{
    Task<T> GetAsync<T>(string endpoint, string? authToken = null);
    Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, string? authToken = null);
    Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, string? authToken = null);
    Task DeleteAsync(string endpoint, string? authToken = null);
}
