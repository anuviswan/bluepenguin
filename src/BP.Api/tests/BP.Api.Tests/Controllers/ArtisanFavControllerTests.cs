using BP.Api.Controllers;
using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BP.Api.Tests.Controllers;

public class ArtisanFavControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithFavs()
    {
        var service = new Mock<IArtisanFavService>();
        var productService = new Mock<IProductService>();
        var imageService = new Mock<IProductImageService>();
        var logger = new Mock<ILogger<ArtisanFavController>>();

        service.Setup(x => x.GetAll()).ReturnsAsync(new[] { "sku-1", "sku-2" });

        productService.Setup(x => x.GetProductBySku("sku-1")).ReturnsAsync(new ProductEntity { SKU = "sku-1", ProductName = "P1", Price = 100, DiscountPrice = 80, DiscountExpiryDate = DateTimeOffset.UtcNow.AddDays(1) });
        productService.Setup(x => x.GetProductBySku("sku-2")).ReturnsAsync(new ProductEntity { SKU = "sku-2", ProductName = "P2", Price = 200 });

        imageService.Setup(x => x.GetPrimaryImageUrlForSkuId("sku-1")).ReturnsAsync("https://blob/sku1.jpg");
        imageService.Setup(x => x.GetPrimaryImageUrlForSkuId("sku-2")).ReturnsAsync((string?)null);

        var controller = new ArtisanFavController(service.Object, productService.Object, imageService.Object, logger.Object);

        var result = await controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var items = Assert.IsAssignableFrom<IEnumerable<ArtisanFavItemResponse>>(ok.Value);
        Assert.Equal(2, items.Count());

        var first = items.First();
        Assert.Equal("sku-1", first.Skuid);
        Assert.Equal("P1", first.ProductName);
        Assert.Equal(80, first.DiscountedPrice);
        Assert.Equal("https://blob/sku1.jpg", first.BlobUrl);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        var service = new Mock<IArtisanFavService>();
        var productService = new Mock<IProductService>();
        var imageService = new Mock<IProductImageService>();
        var logger = new Mock<ILogger<ArtisanFavController>>();

        var controller = new ArtisanFavController(service.Object, productService.Object, imageService.Object, logger.Object);

        var result = await controller.Create(new ArtisanFavRequest(string.Empty));

        Assert.IsType<BadRequestObjectResult>(result);
        service.Verify(x => x.Add(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenSkuIsValid()
    {
        var service = new Mock<IArtisanFavService>();
        var productService = new Mock<IProductService>();
        var imageService = new Mock<IProductImageService>();
        var logger = new Mock<ILogger<ArtisanFavController>>();

        var controller = new ArtisanFavController(service.Object, productService.Object, imageService.Object, logger.Object);

        var result = await controller.Delete("sku-1");

        Assert.IsType<OkResult>(result);
        service.Verify(x => x.Delete("sku-1"), Times.Once);
    }
}
