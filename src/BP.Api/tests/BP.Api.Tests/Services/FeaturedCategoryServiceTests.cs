using BP.Application.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;
using Moq;

namespace BP.Api.Tests.Services;

public class FeaturedCategoryServiceTests
{
    [Fact]
    public async Task GetAll_ReturnsFeaturedCategoryCodes()
    {
        var repository = new Mock<ISectionProductRepository>();
        repository
            .Setup(x => x.GetByPartition(FeaturedCategoryService.FEATURED_CATEGORIES_PARTITION_KEY))
            .ReturnsAsync([
                new SectionProductEntity { PartitionKey = FeaturedCategoryService.FEATURED_CATEGORIES_PARTITION_KEY, RowKey = "cat-1" },
                new SectionProductEntity { PartitionKey = FeaturedCategoryService.FEATURED_CATEGORIES_PARTITION_KEY, RowKey = "cat-2" }
            ]);

        var service = new FeaturedCategoryService(repository.Object);

        var result = await service.GetAll();

        Assert.Equal(["cat-1", "cat-2"], result);
    }

    [Fact]
    public async Task Add_Throws_WhenMaxFeaturedCategoriesReached()
    {
        var repository = new Mock<ISectionProductRepository>();
        repository
            .Setup(x => x.GetByPartition(FeaturedCategoryService.FEATURED_CATEGORIES_PARTITION_KEY))
            .ReturnsAsync([
                new SectionProductEntity { PartitionKey = FeaturedCategoryService.FEATURED_CATEGORIES_PARTITION_KEY, RowKey = "cat-1" },
                new SectionProductEntity { PartitionKey = FeaturedCategoryService.FEATURED_CATEGORIES_PARTITION_KEY, RowKey = "cat-2" },
                new SectionProductEntity { PartitionKey = FeaturedCategoryService.FEATURED_CATEGORIES_PARTITION_KEY, RowKey = "cat-3" },
                new SectionProductEntity { PartitionKey = FeaturedCategoryService.FEATURED_CATEGORIES_PARTITION_KEY, RowKey = "cat-4" }
            ]);

        var service = new FeaturedCategoryService(repository.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.Add("cat-5"));
        repository.Verify(x => x.Add(It.IsAny<SectionProductEntity>()), Times.Never);
    }

    [Fact]
    public async Task Add_AddsFeaturedCategory_WhenBelowLimit()
    {
        var repository = new Mock<ISectionProductRepository>();
        repository
            .Setup(x => x.GetByPartition(FeaturedCategoryService.FEATURED_CATEGORIES_PARTITION_KEY))
            .ReturnsAsync([
                new SectionProductEntity { PartitionKey = FeaturedCategoryService.FEATURED_CATEGORIES_PARTITION_KEY, RowKey = "cat-1" }
            ]);

        var service = new FeaturedCategoryService(repository.Object);

        await service.Add("cat-2");

        repository.Verify(x => x.Add(It.Is<SectionProductEntity>(e =>
            e.PartitionKey == FeaturedCategoryService.FEATURED_CATEGORIES_PARTITION_KEY &&
            e.RowKey == "cat-2")), Times.Once);
    }
}
