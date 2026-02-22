namespace BP.Application.Interfaces.Services;

public interface IShowcaseService
{
    Task<IEnumerable<ShowcaseCategoryResult>> GetTopCategories(int count = 4);
}

public sealed record ShowcaseCategoryResult(string CategoryCode, string CategoryName, string LatestSkuId);
