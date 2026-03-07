using BP.PriceTracker.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using BP.PriceTracker.Services.Types;

namespace BP.PriceTracker.Services.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    // Use DI to inject HttpClient
    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task DeleteAsync(string endpoint, string? authToken = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);

        if (!string.IsNullOrWhiteSpace(authToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        using var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task<ApiResult<T>> GetAsync<T>(string endpoint, string? authToken = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

        if (!string.IsNullOrWhiteSpace(authToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        using var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new ApiResult<T>
            {
                IsSuccess = true,
                Data = result!,
                StatusCode = response.StatusCode
            };
        }

        return new ApiResult<T>
        {
            IsSuccess = false,
            Data = default,
            StatusCode = response.StatusCode,
            ErrorMessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false)
        };
    }

    public async Task<ApiResult<byte[]>> GetBlobAsync(string endpoint, string? authToken = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

        if (!string.IsNullOrWhiteSpace(authToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        using var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            return new ApiResult<byte[]>
            {
                IsSuccess = true,
                Data = bytes,
                StatusCode = response.StatusCode
            };
        }

        return new ApiResult<byte[]>
        {
            IsSuccess = false,
            Data = default,
            StatusCode = response.StatusCode,
            ErrorMessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false)
        };
    }

    public async Task<ApiResult<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, string? authToken = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
        };

        if (!string.IsNullOrWhiteSpace(authToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        using var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new ApiResult<TResponse>
            {
                IsSuccess = true,
                Data = result!,
                StatusCode = response.StatusCode
            };
        }

        return new ApiResult<TResponse>
        {
            IsSuccess = false,
            Data = default,
            StatusCode = response.StatusCode,
            ErrorMessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false)
        };
    }

    public async Task<ApiResult<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, string? authToken = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
        };

        if (!string.IsNullOrWhiteSpace(authToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        using var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new ApiResult<TResponse>
            {
                IsSuccess = true,
                Data = result!,
                StatusCode = response.StatusCode
            };
        }

        return new ApiResult<TResponse>
        {
            IsSuccess = false,
            Data = default,
            StatusCode = response.StatusCode,
            ErrorMessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false)
        };
    }
}
