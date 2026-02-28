using System.Threading.Tasks;
using Xunit;
using Moq;
using BP.Api.Controllers;
using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Application.Interfaces.SkuAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace BP.Api.Tests.Controllers;

public class CategoryControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsAllCategoriesWithProductCounts()
    {
        var mockCategoryService = new Mock<ICategoryService>();
        var mockProductService = new Mock<IProductService>();
        var mockFeaturedCategoryService = new Mock<IFeaturedCategoryService>();
        var mockLogger = new Mock<ILogger<CategoryController>>();

        // Mock categories from enum
        var categories = new List<Category>
        {
            Category.RI,
            Category.NK
        };

        mockCategoryService.Setup(s => s.GetAllCategories())
            .Returns(categories);

        // Mock products by category
        mockProductService.Setup(s => s.GetProductsByCategory("RI"))
            .ReturnsAsync(new List<BP.Domain.Entities.ProductEntity>
            {
                new() { SKU = "RI-RS-FL-ONM-2024-1", PartitionKey = "RI" },
                new() { SKU = "RI-RS-FL-ONM-2024-2", PartitionKey = "RI" },
                new() { SKU = "RI-RS-FL-ONM-2024-3", PartitionKey = "RI" }
            });

        mockProductService.Setup(s => s.GetProductsByCategory("NK"))
            .ReturnsAsync(new List<BP.Domain.Entities.ProductEntity>
            {
                new() { SKU = "NK-RS-FL-ONM-2024-1", PartitionKey = "NK" }
            });

        mockFeaturedCategoryService.Setup(s => s.GetAll())
            .ReturnsAsync(new List<string> { "RI" });

        var controller = new CategoryController(mockCategoryService.Object, mockProductService.Object, mockFeaturedCategoryService.Object, mockLogger.Object);

        var result = await controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        var response = ok.Value as List<CategoryResponse>;
        Assert.NotNull(response);
        Assert.Equal(2, response.Count);

        var ringCategory = response.First(c => c.Id == "RI");
        Assert.NotNull(ringCategory);
        Assert.Equal(3, ringCategory.ProductCount);
        Assert.True(ringCategory.IsFeatured);

        var necklaceCategory = response.First(c => c.Id == "NK");
        Assert.NotNull(necklaceCategory);
        Assert.Equal(1, necklaceCategory.ProductCount);
        Assert.False(necklaceCategory.IsFeatured);
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyProductCountForCategoryWithNoProducts()
    {
        var mockCategoryService = new Mock<ICategoryService>();
        var mockProductService = new Mock<IProductService>();
        var mockFeaturedCategoryService = new Mock<IFeaturedCategoryService>();
        var mockLogger = new Mock<ILogger<CategoryController>>();

        var categories = new List<Category> { Category.RI };

        mockCategoryService.Setup(s => s.GetAllCategories())
            .Returns(categories);

        mockProductService.Setup(s => s.GetProductsByCategory("RI"))
            .ReturnsAsync(new List<BP.Domain.Entities.ProductEntity>());

        mockFeaturedCategoryService.Setup(s => s.GetAll())
            .ReturnsAsync(new List<string>());

        var controller = new CategoryController(mockCategoryService.Object, mockProductService.Object, mockFeaturedCategoryService.Object, mockLogger.Object);

        var result = await controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        var response = ok.Value as List<CategoryResponse>;
        Assert.NotNull(response);
        Assert.Single(response);
        Assert.Equal(0, response[0].ProductCount);
        Assert.False(response[0].IsFeatured);
    }
}
