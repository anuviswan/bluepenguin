using BP.Application.Interfaces.Services;
using BP.Domain.Repository;

namespace BP.Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public BP.Domain.Entities.Product AddProduct(BP.Domain.Entities.Product product)
    {
        return productRepository.Add(product);
    }

    public void DeleteProduct(string sku)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BP.Domain.Entities.Product> GetAllProducts()
    {
        throw new NotImplementedException();
    }

    public int GetItemCountForCollection(string collectionCode, int Year)
    {
        throw new NotImplementedException();
    }

    public BP.Domain.Entities.Product? GetProductBySku(string sku)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BP.Domain.Entities.Product> GetProductsByCategory(string categoryId)
    {
        throw new NotImplementedException();
    }

    public BP.Domain.Entities.Product UpdateProduct(BP.Domain.Entities.Product product)
    {
        throw new NotImplementedException();
    }
}
