namespace BP.PriceTracker.Services.ApiTypes;

public record ValidateUserResponse
{
    public string Token { get; init; } = null!;
    public DateTime Expiration { get; init; }
    public string UserId { get; init; } = null!;
}
