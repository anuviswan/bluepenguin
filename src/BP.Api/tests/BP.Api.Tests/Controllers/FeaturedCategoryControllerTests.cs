using BP.Api.Contracts;
using BP.Api.Controllers;
using BP.Application.Interfaces.Services;
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

public class FeaturedCategoryControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsOk_WithFeaturedCategories()
    {
        var service = new Mock<IFeaturedCategoryService>();
        var productService = new Mock<IProductService>();
        var imageService = new Mock<IProductImageService>();
        var logger = new Mock<ILogger<FeaturedCategoryController>>();

        service.Setup(x => x.GetAll()).ReturnsAsync(["category-1", "category-2"]);

        var controller = new FeaturedCategoryController(service.Object, productService.Object, imageService.Object, logger.Object);

        var result = await controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var categories = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
        Assert.Equal(["category-1", "category-2"], categories);
    }

    [Fact]
    public async Task GetAllForShowcase_ReturnsOk_WithFeaturedCategoryShowcaseResponses()
    {
        var service = new Mock<IFeaturedCategoryService>();
        var productService = new Mock<IProductService>();
        var imageService = new Mock<IProductImageService>();
        var logger = new Mock<ILogger<FeaturedCategoryController>>();

        service.Setup(x => x.GetAll()).ReturnsAsync(["RI"]);
        productService.Setup(x => x.GetProductsByCategory("RI"))
            .ReturnsAsync(new[] { new ProductEntity { SKU = "RI-RS-FL-ONM-2024-9", ProductName = "Ring Product" } });
        imageService.Setup(x => x.GetPrimaryImageUrlForSkuId("RI-RS-FL-ONM-2024-9")).ReturnsAsync("https://blob/ring.jpg");

        var controller = new FeaturedCategoryController(service.Object, productService.Object, imageService.Object, logger.Object);

        var result = await controller.GetAllForShowcase();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var responses = Assert.IsAssignableFrom<IEnumerable<FeaturedCategoryShowcaseResponse>>(okResult.Value);
        Assert.Single(responses);
        var first = responses.First();
        Assert.Equal("RI", first.CategoryCode);
        Assert.Equal("https://blob/ring.jpg", first.BlobUrl);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        var service = new Mock<IFeaturedCategoryService>();
        var productService = new Mock<IProductService>();
        var imageService = new Mock<IProductImageService>();
        var logger = new Mock<ILogger<FeaturedCategoryController>>();

        var controller = new FeaturedCategoryController(service.Object, productService.Object, imageService.Object, logger.Object);

        var result = await controller.Create(new FeaturedCategoryRequest(string.Empty));

        Assert.IsType<BadRequestObjectResult>(result);
        service.Verify(x => x.Add(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenCodeIsValid()
    {
        var service = new Mock<IFeaturedCategoryService>();
        var productService = new Mock<IProductService>();
        var imageService = new Mock<IProductImageService>();
        var logger = new Mock<ILogger<FeaturedCategoryController>>();

        var controller = new FeaturedCategoryController(service.Object, productService.Object, imageService.Object, logger.Object);

        var result = await controller.Delete("category-1");

        Assert.IsType<OkResult>(result);
        service.Verify(x => x.Delete("category-1"), Times.Once);
    }
}
