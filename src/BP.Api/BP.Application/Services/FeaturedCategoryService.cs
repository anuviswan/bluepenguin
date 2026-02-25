using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;

namespace BP.Application.Services;

public class FeaturedCategoryService(ISectionProductRepository sectionProductRepository) : IFeaturedCategoryService
{
    public const string FEATURED_CATEGORIES_PARTITION_KEY = "FeaturedCategories";
    public const int FEATURED_CATEGORIES_LIMIT = 4;

    public async Task Add(string code)
    {
        var featuredCategories = (await sectionProductRepository.GetByPartition(FEATURED_CATEGORIES_PARTITION_KEY).ConfigureAwait(false)).ToList();
        if (featuredCategories.Count >= FEATURED_CATEGORIES_LIMIT)
        {
            throw new InvalidOperationException($"Only {FEATURED_CATEGORIES_LIMIT} featured categories can be stored.");
        }

        if (featuredCategories.Any(x => string.Equals(x.RowKey, code, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Code '{code}' already exists in featured categories.");
        }

        await sectionProductRepository.Add(new SectionProductEntity
        {
            PartitionKey = FEATURED_CATEGORIES_PARTITION_KEY,
            RowKey = code
        }).ConfigureAwait(false);
    }

    public async Task Delete(string code)
    {
        await sectionProductRepository.Delete(FEATURED_CATEGORIES_PARTITION_KEY, code).ConfigureAwait(false);
    }

    public async Task<IEnumerable<string>> GetAll()
    {
        var featuredCategories = await sectionProductRepository
            .GetByPartition(FEATURED_CATEGORIES_PARTITION_KEY)
            .ConfigureAwait(false);

        return featuredCategories.Select(x => x.RowKey);
    }
}
