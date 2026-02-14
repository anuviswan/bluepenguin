namespace BP.Api.Contracts;

/// <summary>
/// Response DTO for product image information.
/// </summary>
public record ProductImageResponse
{
    /// <summary>
    /// Unique identifier of the image.
    /// </summary>
    public string ImageId { get; init; } = null!;

    /// <summary>
    /// Blob name/path in Azure Blob Storage.
    /// </summary>
    public string BlobName { get; init; } = null!;

    /// <summary>
    /// Whether this is the primary/featured image for the product.
    /// </summary>
    public bool IsPrimary { get; init; }

    /// <summary>
    /// Content type of the image (e.g., image/jpeg, image/png).
    /// </summary>
    public string ContentType { get; init; } = null!;

    /// <summary>
    /// URL to download/retrieve this image.
    /// </summary>
    public string? DownloadUrl { get; init; }
}
