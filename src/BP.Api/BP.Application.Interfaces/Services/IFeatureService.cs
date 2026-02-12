using BP.Domain.Entities;

namespace BP.Application.Interfaces.Services;

public interface IFeatureService
{
    Task Add(string featureId, string featureName, string? symbolic = null);
    Task<MetaDataEntity> Update(string featureId, string featureName, string? symbolic = null);
    Task<IEnumerable<MetaDataEntity>> GetAllFeatures();
}
