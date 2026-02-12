using BP.Domain.Entities;

namespace BP.Domain.Repository;

public interface IMetaDataRepository
{
    Task<MetaDataEntity> Add(MetaDataEntity entity);
    Task<MetaDataEntity> Update(MetaDataEntity entity);
    Task Delete(string partitionKey, string rowKey);
    Task<IEnumerable<MetaDataEntity>> GetByPartition(string partitionKey);
    Task<MetaDataEntity> GetByPartitionAndRowKey(string partitionKey, string rowKey);
    Task DeleteAllAsync();
}
