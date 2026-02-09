using BP.Application.Interfaces.Services;
using BP.Domain.Entities;

namespace BP.Application.Services;

public class CollectionService(IMetaDataService metaDataService) : ICollectionService
{
    public const string COLLECTION_KEY = "Collection";
    public IMetaDataService MetaDataService => metaDataService;

    public async Task Add(string code, string title)
    {
        await MetaDataService.Add(new MetaDataEntity
        {
            PartitionKey = COLLECTION_KEY,
            RowKey = code,
            Title = title
        });
    }

    public async Task<IEnumerable<MetaDataEntity>> GetAllCollections()
    {
        return await MetaDataService.GetByPartition(COLLECTION_KEY);
    }
}
