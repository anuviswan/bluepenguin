public class SearchProductsRequest
{
    /// <summary>Categories to match (e.g. "Rings")</summary>
    public IEnumerable<string>? SelectedCategories { get; set; }
    public IEnumerable<string>? SelectedMaterials { get; set; }
    public IEnumerable<string>? SelectedCollections { get; set; }
    public IEnumerable<string>? SelectedFeatures { get; set; }
    public IEnumerable<string>? SelectedYears { get; set; }
}