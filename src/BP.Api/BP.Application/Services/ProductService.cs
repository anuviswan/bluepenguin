using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public BP.Domain.Entities.ProductEntity UpdateProduct(BP.Domain.Entities.ProductEntity product)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Search products using a dictionary of filters. Supported keys (case-insensitive):
    /// - "Category" / "Categories"        -> matches product.PartitionKey
    /// - "MaterialCode" / "Material(s)"  -> matches product.MaterialCode
    /// - "CollectionCode" / "Collections"-> matches product.CollectionCode
    /// - "FeatureCodes" / "Features"     -> matches any feature contained in product.FeatureCodes (CSV)
    /// - "YearCode" / "YearCodes"        -> matches product.YearCode (ToString comparison)
    /// Unknown keys are ignored.
    /// </summary>
    /// <param name="filters">Dictionary where key = attribute name and value = allowed values list.</param>
    /// <returns>Filtered product list.</returns>
    public async Task<IEnumerable<ProductEntity>> SearchProductsAsync(Dictionary<string, IEnumerable<string>> filters)
    {
        if (filters == null || filters.Count == 0)
        {
            return [];
        }

        var allProducts = await productRepository.GetAll();
        IEnumerable<ProductEntity> results = allProducts ?? [];

        foreach (var kvp in filters)
        {
            var key = (kvp.Key ?? string.Empty).Trim().ToLowerInvariant();
            var values = (kvp.Value ?? Enumerable.Empty<string>())
                         .Where(v => !string.IsNullOrWhiteSpace(v))
                         .Select(v => v!.Trim())
                         .ToList();

            if (!values.Any())
            {
                continue;
            }

            switch (key)
            {
                case "SelectedCategories":
                    results = results.Where(p =>
                        !string.IsNullOrWhiteSpace(p.PartitionKey) &&
                        values.Contains(p.PartitionKey, StringComparer.OrdinalIgnoreCase));
                    break;

                case "SelectedMaterials":
                    results = results.Where(p =>
                        !string.IsNullOrWhiteSpace(p.MaterialCode) &&
                        values.Contains(p.MaterialCode, StringComparer.OrdinalIgnoreCase));
                    break;

                case "SelectedCollections":
                    results = results.Where(p =>
                        !string.IsNullOrWhiteSpace(p.CollectionCode) &&
                        values.Contains(p.CollectionCode, StringComparer.OrdinalIgnoreCase));
                    break;

                case "SelectedFeatures":
                    results = results.Where(p =>
                    {
                        if (string.IsNullOrWhiteSpace(p.FeatureCodes))
                        {
                            return false;
                        }
                        var productFeatures = p.FeatureCodes
                                                 .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(f => f.Trim());
                        return productFeatures.Intersect(values, StringComparer.OrdinalIgnoreCase).Any();
                    });
                    break;

                case "SelectedYears":
                    results = results.Where(p =>
                    {
                        var yearStr = p.YearCode.ToString();
                        return !string.IsNullOrWhiteSpace(yearStr) &&
                               values.Contains(yearStr, StringComparer.OrdinalIgnoreCase);
                    });
                    break;

                default:
                    // Unknown filter key - ignore
                    break;
            }
        }

        // Materialize results before returning
        return results.ToList();
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
