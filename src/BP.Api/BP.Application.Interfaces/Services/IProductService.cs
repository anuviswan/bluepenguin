using BP.Domain.Entities;

namespace BP.Application.Interfaces.Services;

public interface IProductService
{
    Task<ProductEntity> AddProduct(ProductEntity product);
    ProductEntity UpdateProduct(ProductEntity product);
    Task DeleteProduct(string sku);
    Task<ProductEntity?> GetProductBySku(string sku);
    Task<IEnumerable<ProductEntity>> GetAllProducts();
    Task<IEnumerable<ProductEntity>> GetProductsByCategory(string categoryId);
    Task<int> GetItemCountForCollection(string collectionCode, int yearCode);
    Task<IEnumerable<ProductEntity>> SearchProductsAsync(IEnumerable<string>? selectedCategories, IEnumerable<string>? selectedMaterials, IEnumerable<string>? selectedCollections, IEnumerable<string>? selectedFeatures, IEnumerable<string>? selectedYears);
}
