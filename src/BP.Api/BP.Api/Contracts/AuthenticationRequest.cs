namespace BP.Api.Contracts;

public record AuthenticationRequest
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}
