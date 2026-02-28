namespace BP.Api.Contracts;

/// <summary>
/// Response DTO for top discounted products in the showcase.
/// </summary>
public record ShowcaseTopDiscountResponse
{
    /// <summary>
    /// SKU identifier of the product.
    /// </summary>
    public string Skuid { get; init; } = null!;

    /// <summary>
    /// Product display name.
    /// </summary>
    public string? ProductName { get; init; }

    /// <summary>
    /// Original price of the product.
    /// </summary>
    public double Price { get; init; }

    /// <summary>
    /// Discounted price when discount is valid; otherwise same as <see cref="Price"/>.
    /// </summary>
    public double DiscountedPrice { get; init; }

    /// <summary>
    /// Primary image URL for the product.
    /// </summary>
    public string? BlobUrl { get; init; }
}
