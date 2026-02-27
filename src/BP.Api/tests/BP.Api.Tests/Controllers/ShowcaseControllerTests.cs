using BP.Api.Controllers;
using BP.Application.Interfaces.Services;
using BP.Api.Contracts;
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
        var mockLogger = new Mock<ILogger<ShowcaseController>>();

        mockShowcaseService.Setup(s => s.GetTopCategories(4))
            .ReturnsAsync(new[] { new ShowcaseCategoryResult("RI", "Rings", "RI-RS-FL-ONM-2024-9") });

        mockImageService.Setup(x => x.GetPrimaryImageUrlForSkuId("RI-RS-FL-ONM-2024-9")).ReturnsAsync("https://blob/sku1.jpg");

        var controller = new ShowcaseController(mockShowcaseService.Object, mockImageService.Object, mockLogger.Object);

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
        var mockLogger = new Mock<ILogger<ShowcaseController>>();

        mockShowcaseService.Setup(s => s.GetTopDiscounts(4))
            .ReturnsAsync(new[] { new ShowcaseDiscountResult("RI-RS-FL-ONM-2024-9", 42.50) });

        var controller = new ShowcaseController(mockShowcaseService.Object, Mock.Of<IProductImageService>(), mockLogger.Object);

        var result = await controller.GetTopDiscounts();

        var ok = Assert.IsType<OkObjectResult>(result);
        var discounts = Assert.IsAssignableFrom<IEnumerable<ShowcaseDiscountResult>>(ok.Value);
        Assert.Single(discounts);
        Assert.Equal("RI-RS-FL-ONM-2024-9", discounts.First().SkuId);
    }


    [Fact]
    public async Task GetTopCollections_ReturnsOkWithPayload()
    {
        var mockShowcaseService = new Mock<IShowcaseService>();
        var mockLogger = new Mock<ILogger<ShowcaseController>>();

        mockShowcaseService.Setup(s => s.GetTopCollections(4))
            .ReturnsAsync(new[] { new ShowcaseCollectionResult("LOVE", 10, "RI-RS-FL-ONM-2024-9") });

        var controller = new ShowcaseController(mockShowcaseService.Object, Mock.Of<IProductImageService>(), mockLogger.Object);

        var result = await controller.GetTopCollections();

        var ok = Assert.IsType<OkObjectResult>(result);
        var collections = Assert.IsAssignableFrom<IEnumerable<ShowcaseCollectionResult>>(ok.Value);
        Assert.Single(collections);
        Assert.Equal("LOVE", collections.First().CollectionCode);
    }

}
