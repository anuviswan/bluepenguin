using System.Threading.Tasks;
using Xunit;
using Moq;
using BP.Api.Controllers;
using BP.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using BP.Api.Contracts;
using Microsoft.AspNetCore.Mvc;
using BP.Domain.Entities;
using System.Collections.Generic;

namespace BP.Api.Tests.Controllers;

public class ProductControllerTests
{
    [Fact]
    public async Task CreateProduct_ReturnsSku_OnSuccess()
    {
        var mockProductService = new Mock<IProductService>();
        var mockSkuService = new Mock<ISkuGeneratorService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        mockSkuService.Setup(s => s.GetSkuCode(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync("RI-RS-FL-ONM-2024-1");

        mockProductService.Setup(s => s.AddProduct(It.IsAny<ProductEntity>()))
            .ReturnsAsync(new ProductEntity { SKU = "RI-RS-FL-ONM-2024-1" });

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockLogger.Object);

        var req = new CreateProductRequest
        {
            Name = "Test",
            Description = "Test Description",
            Price = 10,
            Category = "RI",
            Material = "RS",
            FeatureCodes = new[] { "FL" },
            CollectionCode = "ONM",
            YearCode = 2024,
            SequenceCode = 1
        };

        var result = await controller.CreateProduct(req);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("RI-RS-FL-ONM-2024-1", ok.Value);
    }

    [Fact]
    public async Task GetAllProducts_ReturnsList()
    {
        var mockProductService = new Mock<IProductService>();
        var mockSkuService = new Mock<ISkuGeneratorService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        mockProductService.Setup(s => s.GetAllProducts()).ReturnsAsync(new List<ProductEntity>());

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockLogger.Object);

        var result = await controller.GetAllProducts();
        Assert.IsType<OkObjectResult>(result);
    }
}
