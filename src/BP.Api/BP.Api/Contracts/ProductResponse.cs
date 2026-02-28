namespace BP.Api.Contracts;

/// <summary>
/// Response DTO for product details.
/// </summary>
public record ProductResponse
{
    /// <summary>
    /// SKU identifier of the product.
    /// </summary>
    public string Sku { get; init; } = null!;

    /// <summary>
    /// Category code.
    /// </summary>
    public string? CategoryCode { get; init; }

    /// <summary>
    /// Product name.
    /// </summary>
    public string? ProductName { get; init; }

    /// <summary>
    /// Product description.
    /// </summary>
    public string? ProductDescription { get; init; }

    /// <summary>
    /// Product care instructions.
    /// </summary>
    public IEnumerable<string>? ProductCareInstructions { get; init; }

    /// <summary>
    /// Product specifications.
    /// </summary>
    public IEnumerable<string>? Specifications { get; init; }

    /// <summary>
    /// Original price of the product.
    /// </summary>
    public double Price { get; init; }

    /// <summary>
    /// Effective discounted price (expires if discount is expired).
    /// </summary>
    public double? DiscountPrice { get; init; }

    /// <summary>
    /// Discount expiry date.
    /// </summary>
    public DateTimeOffset? DiscountExpiryDate { get; init; }

    /// <summary>
    /// Current stock count.
    /// </summary>
    public int Stock { get; init; }

    /// <summary>
    /// Material code.
    /// </summary>
    public string? MaterialCode { get; init; }

    /// <summary>
    /// Collection code.
    /// </summary>
    public string? CollectionCode { get; init; }

    /// <summary>
    /// Feature codes (comma-separated or as array).
    /// </summary>
    public string[]? FeatureCodes { get; init; }

    /// <summary>
    /// Year code.
    /// </summary>
    public int YearCode { get; init; }

    /// <summary>
    /// Primary image SAS URL (with read access expiring in 60 minutes).
    /// </summary>
    public string? PrimaryImageUrl { get; init; }

    /// <summary>
    /// Flag indicating if this product is in the artisan favorites list.
    /// </summary>
    public bool IsArtisanFav { get; init; }
}
