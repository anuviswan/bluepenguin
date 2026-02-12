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

    public async Task<MetaDataEntity> Update(string code, string title)
    {
        var collection = await MetaDataService.GetByPartitionAndRowKey(COLLECTION_KEY, code);

        if (collection is not null)
        {
            collection.Title = title;
            return await MetaDataService.Update(collection);
        }

        throw new InvalidOperationException($"Collection with id {code} does not exist.");
    }
}
