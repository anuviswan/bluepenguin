namespace BP.Application.Interfaces.Services;

public interface IShowcaseService
{
    Task<IEnumerable<ShowcaseCategoryResult>> GetTopCategories(int count = 4);
    Task<IEnumerable<ShowcaseDiscountResult>> GetTopDiscounts(int count = 4);
    Task<IEnumerable<ShowcaseCollectionResult>> GetTopCollections(int count = 4);
}

public sealed record ShowcaseCategoryResult(string CategoryCode, string CategoryName, string LatestSkuId);


public sealed record ShowcaseDiscountResult(string SkuId, double DiscountPercentage);


public sealed record ShowcaseCollectionResult(string CollectionCode, int ProductCount, string LatestSkuId);
