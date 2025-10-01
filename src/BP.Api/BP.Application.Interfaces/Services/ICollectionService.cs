namespace BP.Application.Interfaces.Services;

using BP.Application.Interfaces.SkuAttributes;

public interface ICollectionService
{
    IEnumerable<Collection> GetAllCollections();
}
