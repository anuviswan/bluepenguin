namespace BP.Api.Contracts;

/// <summary>
/// Response DTO for top categories returned by the showcase endpoint.
/// </summary>
public record ShowcaseTopCategoryResponse
{
    /// <summary>
    /// Category code (e.g., "RI").
    /// </summary>
    public string CategoryCode { get; init; } = null!;

    /// <summary>
    /// Display name of the category.
    /// </summary>
    public string? CategoryName { get; init; }

    /// <summary>
    /// URL to the primary image of the latest SKU in this category.
    /// </summary>
    public string? BlobUrl { get; init; }
}
