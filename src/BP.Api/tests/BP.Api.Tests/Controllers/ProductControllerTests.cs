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
using System;
using System.Linq;

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
            ProductName = "Test",
            Description = "Test Description",
            ProductCareInstructions = new[] { "Care1", "Care2" },
            Specifications = new[] { "Spec1", "Spec2" },
            Price = 10,
            CategoryCode = "RI",
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

    [Fact]
    public async Task GetAllProducts_PreservesServiceOrdering()
    {
        var mockProductService = new Mock<IProductService>();
        var mockSkuService = new Mock<ISkuGeneratorService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        mockProductService.Setup(s => s.GetAllProducts()).ReturnsAsync(new List<ProductEntity>
        {
            new() { SKU = "SKU-OLDER", PartitionKey = "RI", MaterialCode = "RS", CollectionCode = "ONM", FeatureCodes = "FL", Timestamp = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
            new() { SKU = "SKU-NEWER", PartitionKey = "RI", MaterialCode = "RS", CollectionCode = "ONM", FeatureCodes = "FL", Timestamp = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero) }
        });

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockLogger.Object);

        var result = await controller.GetAllProducts();

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value!;
        var itemsProperty = value.GetType().GetProperty("items");
        Assert.NotNull(itemsProperty);

        var items = itemsProperty!.GetValue(value) as IEnumerable<object>;
        Assert.NotNull(items);

        var first = items!.First();
        var sku = first.GetType().GetProperty("SKU")!.GetValue(first) as string;
        Assert.Equal("SKU-OLDER", sku);
    }
}
