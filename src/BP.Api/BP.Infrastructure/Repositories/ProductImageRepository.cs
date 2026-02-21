using Azure;
using Azure.Data.Tables;
using BP.Domain.Entities;
using BP.Domain.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BP.Infrastructure.Repositories;

public class ProductImageRepository([FromKeyedServices("ProductImages")]TableClient tableClient) : IProductImageRepository
{
    public async Task<ProductImageEntity?> AddProductImage(ProductImageEntity image)
    {
        await tableClient.AddEntityAsync(image);
        return image;
    }


    public async Task<ProductImageEntity?> UpdateProductImage(ProductImageEntity image)
    {
        await tableClient.UpsertEntityAsync(image, TableUpdateMode.Replace);
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

    public async Task DeleteAllAsync()
    {
        await foreach (var entity in tableClient.QueryAsync<ProductImageEntity>())
        {
            try
            {
                await tableClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey);
            }
            catch
            {
                // ignore per-entity failures
            }
        }
    }
}
