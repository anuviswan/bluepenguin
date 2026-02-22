namespace BP.PriceTracker.Services.Types;

public record ProductDto
{
    public string? ProductName { get; init; }
    public string SKU { get; init; } = string.Empty;
    public double Price { get; init; }
    public double? DiscountPrice { get; init; }
    public DateTimeOffset? DiscountExpiryDate { get; init; }
    public int Stock { get; init; }
    public string PartitionKey { get; init; } = string.Empty;
    public string MaterialCode { get; init; } = string.Empty;
    public string CollectionCode { get; init; } = string.Empty;
    public string FeatureCodes { get; init; } = string.Empty;
    public int YearCode { get; init; }

    // Primary image URL (if available) for display in lists
    public string? PrimaryImageUrl { get; init; }
}
