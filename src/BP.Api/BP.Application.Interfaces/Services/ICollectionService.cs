namespace BP.Application.Interfaces.Services;
using BP.Shared.SkuAttributes;

public interface ICollectionService
{
    IEnumerable<Collection> GetAllCollections();
}
