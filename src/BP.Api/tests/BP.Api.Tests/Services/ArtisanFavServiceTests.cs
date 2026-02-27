using BP.Application.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BP.Api.Tests.Services;

public class ArtisanFavServiceTests
{
    [Fact]
    public async Task Add_Throws_WhenMaxFavsReached()
    {
        var repository = new Mock<ISectionProductRepository>();
        repository
            .Setup(x => x.GetByPartition(ArtisanFavService.ARTISAN_FAVS_PARTITION_KEY))
            .ReturnsAsync([
                new SectionProductEntity { PartitionKey = ArtisanFavService.ARTISAN_FAVS_PARTITION_KEY, Sku = "sku-1" },
                new SectionProductEntity { PartitionKey = ArtisanFavService.ARTISAN_FAVS_PARTITION_KEY, Sku = "sku-2" },
                new SectionProductEntity { PartitionKey = ArtisanFavService.ARTISAN_FAVS_PARTITION_KEY, Sku = "sku-3" },
                new SectionProductEntity { PartitionKey = ArtisanFavService.ARTISAN_FAVS_PARTITION_KEY, Sku = "sku-4" }
            ]);

        var service = new ArtisanFavService(repository.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.Add("sku-5"));
        repository.Verify(x => x.Add(It.IsAny<SectionProductEntity>()), Times.Never);
    }

    [Fact]
    public async Task Add_AddsFav_WhenBelowLimit()
    {
        var repository = new Mock<ISectionProductRepository>();
        repository
            .Setup(x => x.GetByPartition(ArtisanFavService.ARTISAN_FAVS_PARTITION_KEY))
            .ReturnsAsync([
                new SectionProductEntity { PartitionKey = ArtisanFavService.ARTISAN_FAVS_PARTITION_KEY, Sku = "sku-1" }
            ]);

        var service = new ArtisanFavService(repository.Object);

        await service.Add("sku-2");

        repository.Verify(x => x.Add(It.Is<SectionProductEntity>(e =>
            e.PartitionKey == ArtisanFavService.ARTISAN_FAVS_PARTITION_KEY &&
            e.Sku == "sku-2")), Times.Once);
    }
}
