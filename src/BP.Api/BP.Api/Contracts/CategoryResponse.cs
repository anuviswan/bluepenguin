namespace BP.Api.Contracts;

/// <summary>
/// DTO for category with product count.
/// </summary>
public record CategoryResponse
{
    /// <summary>
    /// Category identifier.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Category name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Number of products in this category.
    /// </summary>
    public int ProductCount { get; init; }

    /// <summary>
    /// Flag indicating if this category is a featured category.
    /// </summary>
    public bool IsFeatured { get; init; }
}
