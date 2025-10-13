using Azure;
using Azure.Data.Tables;
using BP.Domain.Entities;
using BP.Domain.Repository;

namespace BP.Infrastructure.Repositories;

public class ProductImageRepository(TableClient tableClient) : IProductImageRepository
{
    public async Task<ProductImageEntity?> AddProductImage(ProductImageEntity image)
    {
        await tableClient.AddEntityAsync(image);
        return image;
    }

    public async Task<bool> DeleteProductImage(string sku, string imageId)
    {
        try
        {
            await tableClient.DeleteEntityAsync(sku, imageId);
            return true;
        }
        catch (RequestFailedException)
        {
            return false;
        }
    }

    public async Task<ProductImageEntity?> GetProductImageById(string sku, string imageId)
    {
        try
        {
            var response = await tableClient.GetEntityAsync<ProductImageEntity>(sku, imageId);
            return response.Value;
        }
        catch (RequestFailedException)
        {
            return null;
        }
    }

    public async Task<IEnumerable<ProductImageEntity>> GetProductImagesBySku(string sku)
    {
        var results = new List<ProductImageEntity>();
        await foreach (var entity in tableClient.QueryAsync<ProductImageEntity>(x => x.PartitionKey == sku))
        {
            results.Add(entity);
        }
        return results;
    }
}
