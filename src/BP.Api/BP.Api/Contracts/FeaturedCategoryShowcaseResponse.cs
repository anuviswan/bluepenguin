namespace BP.Api.Contracts;

/// <summary>
/// Response DTO for featured categories in the showcase view.
/// </summary>
public record FeaturedCategoryShowcaseResponse
{
    /// <summary>
    /// Category code identifier.
    /// </summary>
    public string CategoryCode { get; init; } = null!;

    /// <summary>
    /// Display name of the category.
    /// </summary>
    public string? CategoryName { get; init; }

    /// <summary>
    /// Primary image URL of the latest product in the category.
    /// </summary>
    public string? BlobUrl { get; init; }
}
