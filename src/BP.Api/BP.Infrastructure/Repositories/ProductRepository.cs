using Azure.Data.Tables;
using BP.Domain.Entities;
using BP.Domain.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BP.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<ProductEntity>, IProductRepository
{
    public ProductRepository([FromKeyedServices("Product")] TableClient tableClient) : base(tableClient)
    {

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
}
