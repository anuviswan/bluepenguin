using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BP.Application.Services;

public class CollectionService : BP.Application.Interfaces.Services.ICollectionService
{
    private readonly IMetaDataService _metaService;

    public CollectionService(IMetaDataService metaService)
    {
        _metaService = metaService;
    }

    public async Task Add(string id, string title)
    {
        var entity = new MetaDataEntity { PartitionKey = "Collection", RowKey = id, Title = title };
        await _metaService.Add(entity);
    }

    public Task<IEnumerable<MetaDataEntity>> GetAllCollections()
    {
        return _metaService.GetByPartition("Collection");
    }

    // Explicit interface implementations as a fallback if there are alternate interface references
    Task BP.Application.Interfaces.Services.ICollectionService.Add(string id, string title)
        => Add(id, title);

    Task<IEnumerable<MetaDataEntity>> BP.Application.Interfaces.Services.ICollectionService.GetAllCollections()
        => GetAllCollections();
}
