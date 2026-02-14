using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;

namespace BP.Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<ProductEntity> AddProduct(BP.Domain.Entities.ProductEntity product)
    {
        return await productRepository.Add(product);
    }

    public async Task DeleteProduct(string sku)
    {
        var product = await GetProductBySku(sku) ?? throw new Exception($"Product with SKU {sku} not found.");
        await productRepository.Delete(product);
    }

    public async Task<IEnumerable<BP.Domain.Entities.ProductEntity>> GetAllProducts()
    {
        return await productRepository.GetAll();
    }

    public async Task<int> GetItemCountForCollection(string collectionCode, int yearCode)
    {
        var result = await productRepository.GetProductsByCategory(collectionCode, yearCode);
        return result.Count();
    }

    public async Task<BP.Domain.Entities.ProductEntity?> GetProductBySku(string sku)
    {
        var categoryCode = GetCategoryCodeFromSku(sku);
        var product = await productRepository.GetById(categoryCode, sku);
        return product;
    }

    public Task<IEnumerable<BP.Domain.Entities.ProductEntity>> GetProductsByCategory(string categoryId)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductEntity> UpdateProduct(ProductEntity product)
    {
        if (string.IsNullOrWhiteSpace(product?.SKU))
            throw new ArgumentException("SKU is required for update");

        // Get the existing product to check if it exists
        var existingProduct = await GetProductBySku(product.SKU);
        if (existingProduct == null)
            throw new InvalidOperationException($"Product with SKU {product.SKU} not found.");

        // Update the product in repository
        return await productRepository.Update(product);
    }

    /// <summary>
    /// Search products using SearchProductsRequest DTO.
    /// </summary>
    /// <param name="filters">DTO containing lists for supported attributes.</param>
    /// <returns>Filtered product list.</returns>
    public async Task<IEnumerable<ProductEntity>> SearchProductsAsync(
        IEnumerable<string>? selectedCategories,
        IEnumerable<string>? selectedMaterials,
        IEnumerable<string>? selectedCollections,
        IEnumerable<string>? selectedFeatures,
        IEnumerable<string>? selectedYears)
    {
        // Maintain previous behavior: if caller provided no filters at all, return empty set
        if (selectedCategories == null &&
            selectedMaterials == null &&
            selectedCollections == null &&
            selectedFeatures == null &&
            selectedYears == null)
        {
            return Array.Empty<ProductEntity>();
        }

        var allProducts = await productRepository.GetAll();
        IEnumerable<ProductEntity> results = allProducts ?? Array.Empty<ProductEntity>();

        List<string>? Normalize(IEnumerable<string>? seq)
        {
            return seq?
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => v.Trim())
                .ToList();
        }

        var categories = Normalize(selectedCategories);
        if (categories != null && categories.Any())
        {
            results = results.Where(p => !string.IsNullOrWhiteSpace(p.PartitionKey)
                                         && categories.Contains(p.PartitionKey, StringComparer.OrdinalIgnoreCase));
        }

        var materials = Normalize(selectedMaterials);
        if (materials != null && materials.Any())
        {
            results = results.Where(p => !string.IsNullOrWhiteSpace(p.MaterialCode)
                                         && materials.Contains(p.MaterialCode, StringComparer.OrdinalIgnoreCase));
        }

        var collections = Normalize(selectedCollections);
        if (collections != null && collections.Any())
        {
            results = results.Where(p => !string.IsNullOrWhiteSpace(p.CollectionCode)
                                         && collections.Contains(p.CollectionCode, StringComparer.OrdinalIgnoreCase));
        }

        var features = Normalize(selectedFeatures);
        if (features != null && features.Any())
        {
            results = results.Where(p =>
            {
                if (string.IsNullOrWhiteSpace(p.FeatureCodes)) return false;
                var productFeatures = p.FeatureCodes
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(f => f.Trim());
                return productFeatures.Intersect(features, StringComparer.OrdinalIgnoreCase).Any();
            });
        }

        var years = Normalize(selectedYears);
        if (years != null && years.Any())
        {
            results = results.Where(p =>
            {
                var yearStr = p.YearCode.ToString();
                return !string.IsNullOrWhiteSpace(yearStr) && years.Contains(yearStr, StringComparer.OrdinalIgnoreCase);
            });
        }

        return results;
    }



    private string GetCategoryCodeFromSku(string sku)
    {
        // Assuming SKU format is "Category-Material-Features-CollectionYearNumber"
        var parts = sku.Split('-');
        if (parts.Length > 0)
        {
            return parts[0].Substring(0,2); // Return the category part
        }
        throw new ArgumentException("Invalid SKU format");
    }
}
