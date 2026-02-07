using BP.Application.Interfaces.Services;
using BP.Domain.Entities;

namespace BP.Application.Services;

public class FeatureService(IMetaDataService metaDataService) : IFeatureService
{
    public const string FEATURE_KEY = "Feature";
    public IMetaDataService MetaDataService => metaDataService;

    public async Task Add(string featureId, string featureName)
    {
        await MetaDataService.Add(new MetaDataEntity
        {
            PartitionKey = FEATURE_KEY,
            RowKey = featureId,
            Title = featureName
        });
    }

    public async Task<IEnumerable<MetaDataEntity>> GetAllFeatures()
    {
        return await MetaDataService.GetByPartition(FEATURE_KEY);
    }

}
