using BP.Domain.Entities;

namespace BP.Application.Interfaces.Services;

public interface IProductService
{
    Task<Product> AddProduct(Product product);
    Product UpdateProduct(Product product);
    Task DeleteProduct(string sku);
    Task<Product?> GetProductBySku(string sku);
    Task<IEnumerable<Product>> GetAllProducts();
    Task<IEnumerable<Product>> GetProductsByCategory(string categoryId);
    Task<int> GetItemCountForCollection(string collectionCode, int yearCode);
}
