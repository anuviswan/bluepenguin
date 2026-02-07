using BP.Domain.Entities;

namespace BP.Application.Interfaces.Services;

public interface IFeatureService
{
    Task Add(string featureId, string featureName);
    Task<IEnumerable<MetaDataEntity>> GetAllFeatures();
}
