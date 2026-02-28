namespace BP.Api.Contracts;

public record ArtisanFavItemResponse
{
    public string Skuid { get; init; } = null!;
    public string? ProductName { get; init; }
    public double OriginalPrice { get; init; }
    public double DiscountedPrice { get; init; }
    public string? BlobUrl { get; init; }
}
