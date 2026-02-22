namespace BP.Api.Contracts;

/// <summary>
/// DTO for updating an existing product.
/// SKU cannot be changed; attempting to update to an existing SKU will throw an exception.
/// </summary>
public record UpdateProductRequest
{
    /// <summary>
    /// The current SKU of the product (immutable, used for lookup).
    /// </summary>
    public string SKU { get; init; } = null!;

    /// <summary>
    /// Updated product name.
    /// </summary>
    public string? ProductName { get; init; }

    /// <summary>
    /// Updated product description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Updated specifications.
    /// </summary>
    public IEnumerable<string>? Specifications { get; init; }

    /// <summary>
    /// Updated care instructions.
    /// </summary>
    public IEnumerable<string>? ProductCareInstructions { get; init; }

    /// <summary>
    /// Updated product price.
    /// </summary>
    public double? Price { get; init; }

    /// <summary>
    /// Updated discount price.
    /// </summary>
    public double? DiscountPrice { get; init; }

    /// <summary>
    /// Updated discount expiry date.
    /// </summary>
    public DateTimeOffset? DiscountExpiryDate { get; init; }

    /// <summary>
    /// Updated stock quantity.
    /// </summary>
    public int? Stock { get; init; }
}
