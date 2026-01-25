using BP.PriceTracker.Services.Types;

namespace BP.PriceTracker.Services.Interfaces;

public interface IApiService
{
    Task<ApiResult<T>> GetAsync<T>(string endpoint, string? authToken = null);
    Task<ApiResult<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, string? authToken = null);
    Task<ApiResult<TResponse>> PutAsync<TRequest,TResponse>(string endpoint, TRequest data, string? authToken = null);
    Task DeleteAsync(string endpoint, string? authToken = null);
    Task<ApiResult<byte[]>> GetBlobAsync(string endpoint, string? authToken = null);
}
