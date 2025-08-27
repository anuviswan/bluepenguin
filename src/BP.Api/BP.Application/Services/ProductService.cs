using BP.Application.Interfaces.Services;
using BP.Domain.Repository;

namespace BP.Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task AddProduct(BP.Domain.Entities.Product product)
    {
        await productRepository.Add(product);
    }

    public async Task DeleteProduct(string sku)
    {
        var product = await GetProductBySku(sku) ?? throw new Exception($"Product with SKU {sku} not found.");
        await productRepository.Delete(product);
    }

    public async Task<IEnumerable<BP.Domain.Entities.Product>> GetAllProducts()
    {
        return await productRepository.GetAll();
    }

    public async Task<int> GetItemCountForCollection(string collectionCode, int yearCode)
    {
        var result = await productRepository.GetProductsByCategory(collectionCode,yearCode);
        return result.Count();
    }

    public async Task<BP.Domain.Entities.Product?> GetProductBySku(string sku)
    {
        var categoryCode = GetCategoryCodeFromSku(sku);
        var product = await productRepository.GetById(categoryCode, sku);
        return product;
    }

    public Task<IEnumerable<BP.Domain.Entities.Product>> GetProductsByCategory(string categoryId)
    {
        throw new NotImplementedException();
    }

    public BP.Domain.Entities.Product UpdateProduct(BP.Domain.Entities.Product product)
    {
        throw new NotImplementedException();
    }



    private string GetCategoryCodeFromSku(string sku)
    {
        // Assuming SKU format is "Category-Material-Features-CollectionYearNumber"
        var parts = sku.Split('-');
        if (parts.Length > 0)
        {
            return parts[0]; // Return the category part
        }
        throw new ArgumentException("Invalid SKU format");
    }
}
