using BP.Domain.Entities;

namespace BP.Domain.Repository;

public interface ISectionProductRepository
{
    Task<SectionProductEntity> Add(SectionProductEntity entity);
    Task<IEnumerable<SectionProductEntity>> GetByPartition(string partitionKey);
    Task Delete(string partitionKey, string rowKey);
    Task DeleteByRowKey(string rowKey);
}
