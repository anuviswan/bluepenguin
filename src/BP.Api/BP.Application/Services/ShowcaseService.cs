using BP.Application.Interfaces.Services;
using BP.Application.Interfaces.SkuAttributes;
using BP.Domain.Repository;
using System.ComponentModel;

namespace BP.Application.Services;

public class ShowcaseService : IShowcaseService
{
    private readonly IProductRepository _productRepository;

    public ShowcaseService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ShowcaseCategoryResult>> GetTopCategories(int count = 4)
    {
        var safeCount = count <= 0 ? 4 : count;
        var topCategories = await _productRepository.GetTopCategoriesAsync(safeCount);

        return topCategories.Select(x => new ShowcaseCategoryResult(
            x.CategoryCode,
            GetCategoryName(x.CategoryCode),
            x.LatestSkuId));
    }


    public async Task<IEnumerable<ShowcaseDiscountResult>> GetTopDiscounts(int count = 4)
    {
        var safeCount = count <= 0 ? 4 : count;
        var topDiscounts = await _productRepository.GetTopDiscountsAsync(safeCount);

        return topDiscounts.Select(x => new ShowcaseDiscountResult(x.SkuId, x.DiscountPercentage));
    }

    public async Task<IEnumerable<ShowcaseCollectionResult>> GetTopCollections(int count = 4)
    {
        var safeCount = count <= 0 ? 4 : count;
        var topCollections = await _productRepository.GetTopCollectionsAsync(safeCount);

        return topCollections.Select(x => new ShowcaseCollectionResult(x.CollectionCode, x.ProductCount, x.LatestSkuId));
    }

    private static string GetCategoryName(string categoryCode)
    {
        if (Enum.TryParse<Category>(categoryCode, ignoreCase: true, out var category))
        {
            var member = typeof(Category).GetMember(category.ToString()).FirstOrDefault();
            var description = member?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault();

            return description?.Description ?? category.ToString();
        }

        return categoryCode;
    }
}
