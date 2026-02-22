using Azure.Data.Tables;
using BP.Domain.Entities;
using BP.Domain.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BP.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<ProductEntity>, IProductRepository
{
    public ProductRepository([FromKeyedServices("Product")] TableClient tableClient) : base(tableClient)
    {

    }
    public new async Task<IEnumerable<ProductEntity>> GetAll()
    {
        var products = await base.GetAll();
        return products
            .OrderByDescending(p => p.Timestamp ?? DateTimeOffset.MinValue)
            .ThenByDescending(p => p.SKU);
    }

    public async Task<IEnumerable<ProductEntity>> GetProductsByCategory(string categoryCode, int yearCode)
    {
        var queryResults = TableClient.QueryAsync<ProductEntity>(p => p.PartitionKey == categoryCode && p.YearCode == yearCode);

        var results = new List<ProductEntity>();
        await foreach (var entity in queryResults)
        {
            results.Add(entity);

        }

        return results;

    }

    public async Task DeleteAllAsync()
    {
        // Delete all entities in the table by querying and deleting
        await foreach (var entity in TableClient.QueryAsync<ProductEntity>())
        {
            await TableClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey);
        }
    }

    public async Task<bool> CheckIfSkuExistsAsync(string sku)
    {
        var filter = TableClient.CreateQueryFilter<ProductEntity>(e => e.SKU == sku);

        await foreach (var _ in TableClient.QueryAsync<ProductEntity>(filter))
        {
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<TopCategoryStats>> GetTopCategoriesAsync(int count)
    {
        var safeCount = count <= 0 ? 4 : count;
        var categoryRows = new List<(string CategoryCode, string Sku, DateTimeOffset Timestamp)>();

        await foreach (var entity in TableClient.QueryAsync<TableEntity>(select: ["PartitionKey", "SKU", "Timestamp"]))
        {
            if (!entity.TryGetValue("PartitionKey", out var categoryCodeObj) || categoryCodeObj is not string categoryCode || string.IsNullOrWhiteSpace(categoryCode))
            {
                continue;
            }

            var sku = entity.TryGetValue("SKU", out var skuObj) ? skuObj?.ToString() ?? string.Empty : string.Empty;
            var timestamp = entity.Timestamp ?? DateTimeOffset.MinValue;

            categoryRows.Add((categoryCode, sku, timestamp));
        }

        return categoryRows
            .GroupBy(x => x.CategoryCode, StringComparer.OrdinalIgnoreCase)
            .Select(group =>
            {
                var latest = group
                    .OrderByDescending(x => x.Timestamp)
                    .ThenByDescending(x => x.Sku)
                    .First();

                return new
                {
                    CategoryCode = group.Key,
                    ProductCount = group.Count(),
                    LatestTimestamp = latest.Timestamp,
                    LatestSku = latest.Sku
                };
            })
            .OrderByDescending(x => x.ProductCount)
            .ThenByDescending(x => x.LatestTimestamp)
            .ThenBy(x => x.CategoryCode)
            .Take(safeCount)
            .Select(x => new TopCategoryStats(x.CategoryCode, x.ProductCount, x.LatestSku));
    }
}
