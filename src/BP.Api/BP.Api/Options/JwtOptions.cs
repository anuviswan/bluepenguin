namespace BP.Api.Options;

public record JwtOptions
{
    public string Key { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public List<string> Audience { get; init; } = [];
}
