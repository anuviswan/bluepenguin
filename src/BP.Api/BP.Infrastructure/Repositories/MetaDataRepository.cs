using Azure.Data.Tables;
using BP.Domain.Entities;
using BP.Domain.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BP.Infrastructure.Repositories;

public class MetaDataRepository([FromKeyedServices("MetaData")] TableClient tableClient) : IMetaDataRepository
{
    public async Task<MetaDataEntity> Add(MetaDataEntity entity)
    {
        await tableClient.AddEntityAsync(entity).ConfigureAwait(false);
        return entity;
    }

    public async Task<MetaDataEntity> Update(MetaDataEntity entity)
    {
        await tableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace).ConfigureAwait(false);
        return entity;
    }

    public async Task Delete(string partitionKey, string rowKey)
    {
        try
        {
            await tableClient.DeleteEntityAsync(partitionKey, rowKey).ConfigureAwait(false);
        }
        catch
        {
            // ignore
        }
    }

    public async Task DeleteAllAsync()
    {
        await foreach (var entity in tableClient.QueryAsync<MetaDataEntity>().ConfigureAwait(false))
        {
            try
            {
                await tableClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey).ConfigureAwait(false);
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
        await foreach (var e in tableClient.QueryAsync<MetaDataEntity>(x => x.PartitionKey == partitionKey).ConfigureAwait(false))
        {
            results.Add(e);
        }
        return results;
    }

    public async Task<MetaDataEntity> GetByPartitionAndRowKey(string partitionKey,string rowKey)
    {
        var data = await tableClient.GetEntityIfExistsAsync<MetaDataEntity>(partitionKey,rowKey).ConfigureAwait(false);
        if(data.HasValue)
        {
            return data.Value!;
        }
        else
        {
            throw new KeyNotFoundException($"No entity found with PartitionKey: {partitionKey} and RowKey: {rowKey}");
        }
    }
}
