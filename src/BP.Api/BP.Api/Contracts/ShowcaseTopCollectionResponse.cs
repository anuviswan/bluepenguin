namespace BP.Api.Contracts;

/// <summary>
/// Response DTO for top collections in the showcase.
/// </summary>
public record ShowcaseTopCollectionResponse
{
    /// <summary>
    /// Collection code identifier.
    /// </summary>
    public string CollectionCode { get; init; } = null!;

    /// <summary>
    /// Display name of the collection.
    /// </summary>
    public string? CollectionName { get; init; }

    /// <summary>
    /// Number of products in the collection.
    /// </summary>
    public int ProductCount { get; init; }

    /// <summary>
    /// Primary image URL of the latest product in the collection.
    /// </summary>
    public string? BlobUrl { get; init; }
}
