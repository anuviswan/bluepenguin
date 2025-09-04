namespace BP.Application.Interfaces.DTOs;

public record AuthenticationValidation
{
    public bool IsAuthenticated { get; init; }
    public string? Token { get; init; }
    public string UserName { get; init; } = null!;

    public DateTime? Expiration { get; init; }
}
