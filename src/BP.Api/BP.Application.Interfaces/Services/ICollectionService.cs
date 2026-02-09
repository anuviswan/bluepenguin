using BP.Domain.Entities;

namespace BP.Application.Interfaces.Services;

public interface ICollectionService
{
    Task Add(string code, string title);
    Task<IEnumerable<MetaDataEntity>> GetAllCollections();
}
