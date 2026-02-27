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
        }).ConfigureAwait(false);
    }

    public async Task<IEnumerable<MetaDataEntity>> GetAllCollections()
    {
        return await MetaDataService.GetByPartition(COLLECTION_KEY).ConfigureAwait(false);
    }

    public async Task<MetaDataEntity> Update(string code, string title)
    {
        var collection = await MetaDataService.GetByPartitionAndRowKey(COLLECTION_KEY, code).ConfigureAwait(false);

        if (collection is not null)
        {
            collection.Title = title;
            return await MetaDataService.Update(collection).ConfigureAwait(false);
        }

        throw new InvalidOperationException($"Collection with id {code} does not exist.");
    }
}
