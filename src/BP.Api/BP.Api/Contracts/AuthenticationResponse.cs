namespace BP.Api.Contracts;

public record AuthenticationResponse
{
    public string Token { get; init; } = null!;
    public DateTime Expiration { get; init; }
    public string UserId { get; init; } = null!;
}
