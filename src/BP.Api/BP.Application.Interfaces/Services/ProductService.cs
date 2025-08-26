using BP.Domain.Entities;

namespace BP.Application.Interfaces.Services;

public interface IProductService
{
    Product AddProduct(Product product);
    Product UpdateProduct(Product product);
    void DeleteProduct(string sku);
    Product? GetProductBySku(string sku);
    IEnumerable<Product> GetAllProducts();
    IEnumerable<Product> GetProductsByCategory(string categoryId);
    int GetItemCountForCollection(string collectionCode, int Year);
}
