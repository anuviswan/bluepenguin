namespace BP.Api.Contracts;

/// <summary>
/// Response DTO for product image information.
/// </summary>
public record ProductImageDetailsResponse
{
    /// <summary>
    /// Unique identifier of the image.
    /// </summary>
    public string ImageId { get; init; } = null!;

    /// <summary>
    /// SAS URL to access/download the image (expires in 60 minutes).
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// Flag indicating if this is the primary/featured image for the product.
    /// </summary>
    public bool IsPrimary { get; init; }
}
