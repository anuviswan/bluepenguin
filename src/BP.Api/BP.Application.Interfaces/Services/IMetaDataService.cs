using BP.Domain.Entities;

namespace BP.Application.Interfaces.Services;

public interface IMetaDataService
{
    Task<MetaDataEntity> Add(MetaDataEntity entity);
    Task<MetaDataEntity> Update(MetaDataEntity entity);
    Task<IEnumerable<MetaDataEntity>> GetByPartition(string partitionKey);

    Task<MetaDataEntity> GetByPartitionAndRowKey(string partitionKey,string rowKey);
}
