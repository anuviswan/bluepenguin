using BP.Application.Interfaces.Services;
using BP.Domain.Entities;

namespace BP.Application.Services;

public class FeatureService(IMetaDataService metaDataService) : IFeatureService
{
    public const string FEATURE_KEY = "Feature";
    public IMetaDataService MetaDataService => metaDataService;

    public async Task Add(string featureId, string featureName, string? symbolic = null)
    {
        await MetaDataService.Add(new MetaDataEntity
        {
            PartitionKey = FEATURE_KEY,
            RowKey = featureId,
            Title = featureName,
            Notes = symbolic
        });
    }

    public async Task<IEnumerable<MetaDataEntity>> GetAllFeatures()
    {
        return await MetaDataService.GetByPartition(FEATURE_KEY);
    }

    public async Task<MetaDataEntity> Update(string featureId, string featureName, string? symbolic = null)
    {
        var feature = await MetaDataService.GetByPartitionAndRowKey(FEATURE_KEY, featureId);

        if (feature is not null)
        {
            feature.Title = featureName;
            feature.Notes = symbolic;
            return await MetaDataService.Update(feature);
        }

        throw new InvalidOperationException($"Feature with id {featureId} does not exist.");
    }
}
