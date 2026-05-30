using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;

namespace BP.Application.Services;

public class ArtisanFavService : IArtisanFavService
{
    public const string ARTISAN_FAVS_PARTITION_KEY = "ArtisanFavs";
    private readonly ISectionProductRepository _sectionProductRepository;
    private readonly int _limit;

    public ArtisanFavService(ISectionProductRepository sectionProductRepository, Microsoft.Extensions.Options.IOptions<BP.Application.Interfaces.Options.ArtisanFavOptions> options)
    {
        _sectionProductRepository = sectionProductRepository;
        _limit = options.Value.Limit;
    }

    public async Task Add(string sku)
    {
        var artisanFavs = (await _sectionProductRepository.GetByPartition(ARTISAN_FAVS_PARTITION_KEY).ConfigureAwait(false)).ToList();
        if (artisanFavs.Count >= _limit)
        {
            throw new InvalidOperationException($"Only {_limit} artisan favs can be stored.");
        }

        if (artisanFavs.Any(x => string.Equals(x.Sku, sku, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Sku '{sku}' already exists in artisan favs.");
        }

        await _sectionProductRepository.Add(new SectionProductEntity
        {
            PartitionKey = ARTISAN_FAVS_PARTITION_KEY,
            Sku = sku
        }).ConfigureAwait(false);
    }

    public async Task<IEnumerable<string>> GetAll()
    {
        var artisanFavs = await _sectionProductRepository.GetByPartition(ARTISAN_FAVS_PARTITION_KEY).ConfigureAwait(false);
        return artisanFavs.Select(x => x.Sku);
    }

    public async Task<IEnumerable<string>> GetLatest(int count)
    {
        var artisanFavs = (await _sectionProductRepository.GetByPartition(ARTISAN_FAVS_PARTITION_KEY).ConfigureAwait(false))
            .OrderByDescending(x => x.Timestamp)
            .Take(count)
            .Select(x => x.Sku)
            .ToList();
        return artisanFavs;
    }

    public async Task Delete(string sku)
    {
        await _sectionProductRepository.Delete(ARTISAN_FAVS_PARTITION_KEY, sku).ConfigureAwait(false);
    }
}
