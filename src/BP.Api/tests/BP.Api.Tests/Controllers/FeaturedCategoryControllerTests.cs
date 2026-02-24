using BP.Api.Contracts;
using BP.Api.Controllers;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BP.Api.Tests.Controllers;

public class FeaturedCategoryControllerTests
{
    [Fact]
    public async Task Create_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        var service = new Mock<IFeaturedCategoryService>();
        var logger = new Mock<ILogger<FeaturedCategoryController>>();

        var controller = new FeaturedCategoryController(service.Object, logger.Object);

        var result = await controller.Create(new FeaturedCategoryRequest(string.Empty));

        Assert.IsType<BadRequestObjectResult>(result);
        service.Verify(x => x.Add(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenCodeIsValid()
    {
        var service = new Mock<IFeaturedCategoryService>();
        var logger = new Mock<ILogger<FeaturedCategoryController>>();

        var controller = new FeaturedCategoryController(service.Object, logger.Object);

        var result = await controller.Delete("category-1");

        Assert.IsType<OkResult>(result);
        service.Verify(x => x.Delete("category-1"), Times.Once);
    }
}
