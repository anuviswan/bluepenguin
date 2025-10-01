using BP.PriceTracker.Services.Types;

namespace BP.PriceTracker.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<IEnumerable<Material>> GetMaterialsAsync();
    Task<IEnumerable<Feature>> GetFeaturesAsync();
    Task<IEnumerable<Collection>> GetCollectionsAsync();

}
