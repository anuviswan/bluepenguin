using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;

namespace BP.Application.Services;

public class MetaDataService(IMetaDataRepository metaRepo) : IMetaDataService
{
    public async Task<MetaDataEntity> Add(MetaDataEntity entity)
    {
        return await metaRepo.Add(entity);
    }

    public async Task<MetaDataEntity> Update(MetaDataEntity entity)
    {
        return await metaRepo.Update(entity);
    }

    public async Task<IEnumerable<MetaDataEntity>> GetByPartition(string partitionKey)
    {
        return await metaRepo.GetByPartition(partitionKey);
    }

    public async Task<MetaDataEntity> GetByPartitionAndRowKey(string partitionKey, string rowKey)
    {
        return await metaRepo.GetByPartitionAndRowKey(partitionKey, rowKey);
    }
}
