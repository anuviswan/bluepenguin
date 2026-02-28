namespace BP.Api.Contracts;

/// <summary>
/// DTO for product in list/search results.
/// </summary>
public record ProductListItemResponse
{
    /// <summary>
    /// Product SKU.
    /// </summary>
    public required string Sku { get; init; }

    /// <summary>
    /// Category code.
    /// </summary>
    public required string CategoryCode { get; init; }

    /// <summary>
    /// Product name.
    /// </summary>
    public required string ProductName { get; init; }

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
    /// Original price.
    /// </summary>
    public double Price { get; init; }

    /// <summary>
    /// Discounted price (null if no valid discount).
    /// </summary>
    public double? DiscountPrice { get; init; }

    /// <summary>
    /// Discount expiry date.
    /// </summary>
    public DateTimeOffset? DiscountExpiryDate { get; init; }

    /// <summary>
    /// Available stock.
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
    /// Feature codes.
    /// </summary>
    public IEnumerable<string>? FeatureCodes { get; init; }

    /// <summary>
    /// Year code.
    /// </summary>
    public int YearCode { get; init; }

    /// <summary>
    /// SAS URL of primary product image.
    /// </summary>
    public string? PrimaryImageUrl { get; init; }

    /// <summary>
    /// Indicates if product is an artisan favorite.
    /// </summary>
    public bool IsArtisanFav { get; init; }
}
