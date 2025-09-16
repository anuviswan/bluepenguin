using BP.PriceTracker.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace BP.PriceTracker.Services.Services;

public class ApiService : IApiService
{
    private static readonly HttpClient _httpClient = new();

    public async Task DeleteAsync(string endpoint, string? authToken = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);

        if (!string.IsNullOrWhiteSpace(authToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<T> GetAsync<T>(string endpoint, string? authToken = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

        if (!string.IsNullOrWhiteSpace(authToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result!;
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, string? authToken = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
        };

        if (!string.IsNullOrWhiteSpace(authToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result!;
    }

    public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, string? authToken = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
        };

        if (!string.IsNullOrWhiteSpace(authToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result!;
    }
}
