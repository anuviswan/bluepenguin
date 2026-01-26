using BP.PriceTracker.Services.Types;

namespace BP.PriceTracker.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<IEnumerable<Material>> GetMaterialsAsync();
    Task<IEnumerable<Feature>> GetFeaturesAsync();
    Task<IEnumerable<Collection>> GetCollectionsAsync();

    Task<IEnumerable<ProductDto>> SearchProductsAsync(SearchFilter filters);

    // Fetch primary image (data URL) for a SKU if available
    Task<string?> GetPrimaryImageForSkuAsync(string skuId);

}
