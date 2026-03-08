using BP.Api.Controllers;
using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace BP.Api.Tests.Controllers;

public class ImageSearchControllerTests
{
    [Fact]
    public async Task FindClosestProducts_ReturnsBadRequest_WhenImageMissing()
    {
        var imageService = new Mock<IProductImageService>();
        var productService = new Mock<IProductService>();
        var fileUploadService = new Mock<IFileUploadService>();
        var logger = new Mock<ILogger<ImageSearchController>>();

        var controller = new ImageSearchController(imageService.Object, productService.Object, fileUploadService.Object, logger.Object);

        var result = await controller.FindClosestProducts(null!, 5);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Image file is required.", badRequest.Value);
    }

    [Fact]
    public async Task FindClosestProducts_ReturnsMatchedProducts()
    {
        var imageService = new Mock<IProductImageService>();
        var productService = new Mock<IProductService>();
        var fileUploadService = new Mock<IFileUploadService>();
        var logger = new Mock<ILogger<ImageSearchController>>();

        var controller = new ImageSearchController(imageService.Object, productService.Object, fileUploadService.Object, logger.Object);

        var stream = new MemoryStream(Encoding.UTF8.GetBytes("test-image"));
        var formFile = new FormFile(stream, 0, stream.Length, "image", "search.jpg")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg"
        };

        var matches = new[]
        {
            new ImageSimilarityMatch("RI-1", "IMG-1", "blob-1", 0.98),
            new ImageSimilarityMatch("CU-2", "IMG-2", "blob-2", 0.93)
        };

        imageService
            .Setup(s => s.FindClosestProductImagesAsync(It.IsAny<Stream>(), 5))
            .ReturnsAsync(matches);

        productService
            .Setup(s => s.GetProductBySku("RI-1"))
            .ReturnsAsync(new ProductEntity { SKU = "RI-1", ProductName = "Ring", Price = 100, DiscountPrice = 90 });

        productService
            .Setup(s => s.GetProductBySku("CU-2"))
            .ReturnsAsync(new ProductEntity { SKU = "CU-2", ProductName = "Cuff", Price = 120, DiscountPrice = null });

        fileUploadService
            .Setup(s => s.GetBlobUrlAsync("blob-1"))
            .ReturnsAsync("https://cdn/blob-1");

        fileUploadService
            .Setup(s => s.GetBlobUrlAsync("blob-2"))
            .ReturnsAsync("https://cdn/blob-2");

        var result = await controller.FindClosestProducts(formFile, 5);

        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);
        Assert.Equal(2, payload.Count());
    }
}
