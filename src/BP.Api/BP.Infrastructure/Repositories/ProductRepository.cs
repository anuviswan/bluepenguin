using Azure.Data.Tables;
using BP.Domain.Entities;
using BP.Domain.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BP.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository([FromKeyedServices("Product")] TableClient tableClient) : base(tableClient)
    {

    }
    public async Task<IEnumerable<Product>> GetProductsByCategory(string categoryCode, int yearCode)
    {
        var queryResults = TableClient.QueryAsync<Product>(p => p.PartitionKey == categoryCode && p.YearCode == yearCode);

        var results = new List<Product>();
        await foreach (var entity in queryResults)
        {
            results.Add(entity);

        }

        return results;

    }
}
