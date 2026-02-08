using BP.Application.Interfaces.Services;
using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace BP.Application.Services;

public class FeatureService : IFeatureService
{
    private readonly IMetaDataService _metaService;

    public FeatureService(IMetaDataService metaService)
    {
        _metaService = metaService;
    }

    public async Task Add(string featureId, string featureName)
    {
        var entity = new MetaDataEntity
        {
            PartitionKey = "Feature",
            RowKey = featureId,
            Title = featureName
        };
        await _metaService.Add(entity);
    }

    public async Task<IEnumerable<MetaDataEntity>> GetAllFeatures()
    {
        return await _metaService.GetByPartition("Feature");
    }
}
