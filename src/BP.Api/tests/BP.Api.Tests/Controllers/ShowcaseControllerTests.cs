using BP.Api.Controllers;
using BP.Application.Interfaces.Services;
using BP.Api.Contracts;
using BP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BP.Api.Tests.Controllers;

public class ShowcaseControllerTests
{
    [Fact]
    public async Task GetTopCategories_ReturnsOkWithPayload()
    {
        var mockShowcaseService = new Mock<IShowcaseService>();
        var mockImageService = new Mock<IProductImageService>();
        var mockProductService = new Mock<IProductService>();
        var mockCollectionService = new Mock<ICollectionService>();
        var mockLogger = new Mock<ILogger<ShowcaseController>>();

        mockShowcaseService.Setup(s => s.GetTopCategories(4))
            .ReturnsAsync(new[] { new ShowcaseCategoryResult("RI", "Rings", "RI-RS-FL-ONM-2024-9") });

        mockImageService.Setup(x => x.GetPrimaryImageUrlForSkuId("RI-RS-FL-ONM-2024-9")).ReturnsAsync("https://blob/sku1.jpg");

        var controller = new ShowcaseController(mockShowcaseService.Object, mockImageService.Object, mockProductService.Object, mockCollectionService.Object, mockLogger.Object);

        var result = await controller.GetTopCategories();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var categories = Assert.IsAssignableFrom<IEnumerable<ShowcaseTopCategoryResponse>>(ok.Value);
        Assert.Single(categories);
        Assert.Equal("RI", categories.First().CategoryCode);
        Assert.Equal("https://blob/sku1.jpg", categories.First().BlobUrl);
    }

    [Fact]
    public async Task GetTopDiscounts_ReturnsOkWithPayload()
    {
        var mockShowcaseService = new Mock<IShowcaseService>();
        var mockProductService = new Mock<IProductService>();
        var mockImageService = new Mock<IProductImageService>();
        var mockCollectionService = new Mock<ICollectionService>();
        var mockLogger = new Mock<ILogger<ShowcaseController>>();

        mockShowcaseService.Setup(s => s.GetTopDiscounts(4))
            .ReturnsAsync(new[] { new ShowcaseDiscountResult("RI-RS-FL-ONM-2024-9", 42.50) });

        mockProductService.Setup(x => x.GetProductBySku("RI-RS-FL-ONM-2024-9")).ReturnsAsync(new ProductEntity { SKU = "RI-RS-FL-ONM-2024-9", ProductName = "P1", Price = 100, DiscountPrice = 80, DiscountExpiryDate = DateTimeOffset.UtcNow.AddDays(1) });
        mockImageService.Setup(x => x.GetPrimaryImageUrlForSkuId("RI-RS-FL-ONM-2024-9")).ReturnsAsync("https://blob/sku1.jpg");

        var controller = new ShowcaseController(mockShowcaseService.Object, mockImageService.Object, mockProductService.Object, mockCollectionService.Object, mockLogger.Object);

        var result = await controller.GetTopDiscounts();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var discounts = Assert.IsAssignableFrom<IEnumerable<ShowcaseTopDiscountResponse>>(ok.Value);
        Assert.Single(discounts);
        var first = discounts.First();
        Assert.Equal("RI-RS-FL-ONM-2024-9", first.Skuid);
        Assert.Equal("P1", first.ProductName);
        Assert.Equal(80, first.DiscountedPrice);
        Assert.Equal("https://blob/sku1.jpg", first.BlobUrl);
    }


    [Fact]
    public async Task GetTopCollections_ReturnsOkWithPayload()
    {
        var mockShowcaseService = new Mock<IShowcaseService>();
        var mockImageService = new Mock<IProductImageService>();
        var mockProductService = new Mock<IProductService>();
        var mockCollectionService = new Mock<ICollectionService>();
        var mockLogger = new Mock<ILogger<ShowcaseController>>();

        mockShowcaseService.Setup(s => s.GetTopCollections(4))
            .ReturnsAsync(new[] { new ShowcaseCollectionResult("ONM", 10, "RI-RS-FL-ONM-2024-9") });

        mockCollectionService.Setup(x => x.GetAllCollections())
            .ReturnsAsync(new[] 
            { 
                new MetaDataEntity { PartitionKey = "Collection", RowKey = "ONM", Title = "Onam" }
            });

        mockImageService.Setup(x => x.GetPrimaryImageUrlForSkuId("RI-RS-FL-ONM-2024-9")).ReturnsAsync("https://blob/sku1.jpg");

        var controller = new ShowcaseController(mockShowcaseService.Object, mockImageService.Object, mockProductService.Object, mockCollectionService.Object, mockLogger.Object);

        var result = await controller.GetTopCollections();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var collections = Assert.IsAssignableFrom<IEnumerable<ShowcaseTopCollectionResponse>>(ok.Value);
        Assert.Single(collections);
        var first = collections.First();
        Assert.Equal("ONM", first.CollectionCode);
        Assert.Equal("Onam", first.CollectionName);
        Assert.Equal(10, first.ProductCount);
        Assert.Equal("https://blob/sku1.jpg", first.BlobUrl);
    }

}
