namespace BP.PriceTracker.Services.Options;

public record ApiSettings
{
    public string BaseUrl { get; init; } = string.Empty;
    public string ValidateUserEndpoint { get; init; } = string.Empty;
    public string GetCategoriesEndpoint { get; init; } = string.Empty;
    public string GetMaterialsEndpoint { get; init; } = string.Empty;
    public string GetFeaturesEndpoint { get; init; } = string.Empty;
    public string GetCollectionsEndpoint { get; init; } = string.Empty;
}
