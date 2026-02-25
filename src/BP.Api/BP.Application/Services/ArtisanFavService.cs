using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;

namespace BP.Application.Services;

public class ArtisanFavService(ISectionProductRepository sectionProductRepository) : IArtisanFavService
{
    public const string ARTISAN_FAVS_PARTITION_KEY = "ArtisanFavs";
    public const int ARTISAN_FAVS_LIMIT = 4;

    public async Task Add(string sku)
    {
        var artisanFavs = (await sectionProductRepository.GetByPartition(ARTISAN_FAVS_PARTITION_KEY).ConfigureAwait(false)).ToList();
        if (artisanFavs.Count >= ARTISAN_FAVS_LIMIT)
        {
            throw new InvalidOperationException($"Only {ARTISAN_FAVS_LIMIT} artisan favs can be stored.");
        }

        if (artisanFavs.Any(x => string.Equals(x.Sku, sku, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Sku '{sku}' already exists in artisan favs.");
        }

        await sectionProductRepository.Add(new SectionProductEntity
        {
            PartitionKey = ARTISAN_FAVS_PARTITION_KEY,
            Sku = sku
        }).ConfigureAwait(false);
    }

    public async Task<IEnumerable<string>> GetAll()
    {
        var artisanFavs = await sectionProductRepository.GetByPartition(ARTISAN_FAVS_PARTITION_KEY).ConfigureAwait(false);
        return artisanFavs.Select(x => x.Sku);
    }

    public async Task Delete(string sku)
    {
        await sectionProductRepository.Delete(ARTISAN_FAVS_PARTITION_KEY, sku).ConfigureAwait(false);
    }
}
