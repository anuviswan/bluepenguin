using BP.Api.Controllers;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BP.Api.Tests.Controllers;

public class ShowcaseControllerTests
{
    [Fact]
    public async Task GetTopCategories_ReturnsOkWithPayload()
    {
        var mockShowcaseService = new Mock<IShowcaseService>();
        var mockLogger = new Mock<ILogger<ShowcaseController>>();

        mockShowcaseService.Setup(s => s.GetTopCategories(4))
            .ReturnsAsync([
                new ShowcaseCategoryResult("RI", "Rings", "RI-RS-FL-ONM-2024-9")
            ]);

        var controller = new ShowcaseController(mockShowcaseService.Object, mockLogger.Object);

        var result = await controller.GetTopCategories();

        var ok = Assert.IsType<OkObjectResult>(result);
        var categories = Assert.IsAssignableFrom<IEnumerable<ShowcaseCategoryResult>>(ok.Value);
        Assert.Single(categories);
        Assert.Equal("RI", categories.First().CategoryCode);
    }

    [Fact]
    public async Task GetTopDiscounts_ReturnsOkWithPayload()
    {
        var mockShowcaseService = new Mock<IShowcaseService>();
        var mockLogger = new Mock<ILogger<ShowcaseController>>();

        mockShowcaseService.Setup(s => s.GetTopDiscounts(4))
            .ReturnsAsync([
                new ShowcaseDiscountResult("RI-RS-FL-ONM-2024-9", 42.50)
            ]);

        var controller = new ShowcaseController(mockShowcaseService.Object, mockLogger.Object);

        var result = await controller.GetTopDiscounts();

        var ok = Assert.IsType<OkObjectResult>(result);
        var discounts = Assert.IsAssignableFrom<IEnumerable<ShowcaseDiscountResult>>(ok.Value);
        Assert.Single(discounts);
        Assert.Equal("RI-RS-FL-ONM-2024-9", discounts.First().SkuId);
    }

}
