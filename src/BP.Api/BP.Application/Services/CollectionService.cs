using BP.Application.Interfaces.Services;
using BP.Application.Interfaces.SkuAttributes;

namespace BP.Application.Services;

public class CollectionService : ICollectionService
{
    public IEnumerable<Collection> GetAllCollections()
    {
        return Enum.GetValues(typeof(Collection)).Cast<Collection>();
    }
}
