namespace BP.PriceTracker.Services.ApiTypes;

public record ValidateUserResponse
{
    public string AuthToken { get; init; } = string.Empty;
    public bool IsAuthenticated { get; init; }
}
