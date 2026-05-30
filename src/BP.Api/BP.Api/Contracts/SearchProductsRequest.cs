using BP.Domain.Entities;

public class SearchProductsRequest
{
    /// <summary>Categories to match (e.g. "Rings")</summary>
    public IEnumerable<string>? SelectedCategories { get; set; }
    public IEnumerable<string>? SelectedMaterials { get; set; }
    public IEnumerable<string>? SelectedCollections { get; set; }
    public IEnumerable<string>? SelectedFeatures { get; set; }
    public IEnumerable<string>? SelectedYears { get; set; }

    /// <summary>Partial or full product name to match (case-insensitive)</summary>
    public string? PartialProductName { get; set; }

    /// <summary>Sort order for results (default: Newest)</summary>
    public ProductSortOrder SortOrder { get; set; } = ProductSortOrder.Newest;
}
