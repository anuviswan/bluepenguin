using System.Net;

namespace BP.PriceTracker.Services.Types;

public record ApiResult<T>
{
    public bool IsSuccess { get; init; }
    public HttpStatusCode StatusCode { get; init; }
    public T? Data { get; init; }
    public string? ErrorMessage { get; init; }
}
