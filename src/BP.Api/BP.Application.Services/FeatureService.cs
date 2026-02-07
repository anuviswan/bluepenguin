using BP.Application.Interfaces.Services;
using BP.Domain.Repository;
using System.Linq;

namespace BP.Application.Services;

public class FeatureService : IFeatureService
{
    private readonly IMetaDataRepository _metaRepo;

    public FeatureService(IMetaDataRepository metaRepo)
    {
        _metaRepo = metaRepo;
    }

    public IEnumerable<string> GetAllFeatures()
    {
        var items = _metaRepo.GetByPartition("Feature").GetAwaiter().GetResult();
        return items.Select(i => i.RowKey);
    }
}
