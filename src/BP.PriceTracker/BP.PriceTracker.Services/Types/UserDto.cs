namespace BP.PriceTracker.Services.Types;

public record UserDto
{
    public string AuthToken { get; init; } = string.Empty;
    public bool IsAuthenticated { get; init; }
}
