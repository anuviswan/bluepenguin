using Azure.Data.Tables;
using BP.Domain.Entities;
using BP.Domain.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BP.Infrastructure.Repositories;

public class SectionProductRepository([FromKeyedServices("SectionProducts")] TableClient tableClient) : ISectionProductRepository
{
    public async Task<SectionProductEntity> Add(SectionProductEntity entity)
    {
        await tableClient.AddEntityAsync(entity).ConfigureAwait(false);
        return entity;
    }

    public async Task<IEnumerable<SectionProductEntity>> GetByPartition(string partitionKey)
    {
        var entities = new List<SectionProductEntity>();
        await foreach (var entity in tableClient.QueryAsync<SectionProductEntity>(x => x.PartitionKey == partitionKey).ConfigureAwait(false))
        {
            entities.Add(entity);
        }

        return entities;
    }

    public async Task Delete(string partitionKey, string rowKey)
    {
        await tableClient.DeleteEntityAsync(partitionKey, rowKey).ConfigureAwait(false);
    }
}
