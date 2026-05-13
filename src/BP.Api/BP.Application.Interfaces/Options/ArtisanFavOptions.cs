namespace BP.Application.Interfaces.Options;

public record ArtisanFavOptions
{
    public int Limit { get; init; } = 50;
}
