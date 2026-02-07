using Azure.Data.Tables;
using BP.Domain.Entities;
using BP.Domain.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BP.Infrastructure.Repositories;

public class MetaDataRepository([FromKeyedServices("MetaData")] TableClient tableClient) : IMetaDataRepository
{
    public async Task<MetaDataEntity> Add(MetaDataEntity entity)
    {
        await tableClient.AddEntityAsync(entity);
        return entity;
    }

    public async Task Delete(string partitionKey, string rowKey)
    {
        try
        {
            await tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
        catch
        {
            // ignore
        }
    }

    public async Task DeleteAllAsync()
    {
        await foreach (var entity in tableClient.QueryAsync<MetaDataEntity>())
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

    public async Task<IEnumerable<MetaDataEntity>> GetByPartition(string partitionKey)
    {
        var results = new List<MetaDataEntity>();
        await foreach (var e in tableClient.QueryAsync<MetaDataEntity>(x => x.PartitionKey == partitionKey))
        {
            results.Add(e);
        }
        return results;
    }
}
