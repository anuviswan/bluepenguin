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
        var mockImageService = new Mock<IProductImageService>();
        var mockArtisanFavService = new Mock<IArtisanFavService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        mockSkuService.Setup(s => s.GetSkuCode(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync("RI-RS-FL-ONM-2024-1");

        mockProductService.Setup(s => s.AddProduct(It.IsAny<ProductEntity>()))
            .ReturnsAsync(new ProductEntity { SKU = "RI-RS-FL-ONM-2024-1" });

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockImageService.Object, mockArtisanFavService.Object, mockLogger.Object);

        var req = new CreateProductRequest
        {
            ProductName = "Test",
            Description = "Test Description",
            ProductCareInstructions = new[] { "Care1", "Care2" },
            Specifications = new[] { "Spec1", "Spec2" },
            Price = 10,
            DiscountPrice = 7.5,
            DiscountExpiryDate = DateTimeOffset.UtcNow.AddDays(10),
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
        var mockImageService = new Mock<IProductImageService>();
        var mockArtisanFavService = new Mock<IArtisanFavService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        mockProductService.Setup(s => s.GetAllProducts()).ReturnsAsync(new List<ProductEntity>());
        mockArtisanFavService.Setup(s => s.GetAll()).ReturnsAsync(new List<string>());

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockImageService.Object, mockArtisanFavService.Object, mockLogger.Object);

        var result = await controller.GetAllProducts();
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetAllProducts_PreservesServiceOrdering()
    {
        var mockProductService = new Mock<IProductService>();
        var mockSkuService = new Mock<ISkuGeneratorService>();
        var mockImageService = new Mock<IProductImageService>();
        var mockArtisanFavService = new Mock<IArtisanFavService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        mockProductService.Setup(s => s.GetAllProducts()).ReturnsAsync(new List<ProductEntity>
        {
            new() { SKU = "SKU-OLDER", PartitionKey = "RI", MaterialCode = "RS", CollectionCode = "ONM", FeatureCodes = "FL", Timestamp = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
            new() { SKU = "SKU-NEWER", PartitionKey = "RI", MaterialCode = "RS", CollectionCode = "ONM", FeatureCodes = "FL", Timestamp = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero) }
        });

        mockImageService.Setup(s => s.GetPrimaryImageUrlForSkuId(It.IsAny<string>()))
            .ReturnsAsync("https://blob.sas.url/image.jpg");

        mockArtisanFavService.Setup(s => s.GetAll()).ReturnsAsync(new List<string>());

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockImageService.Object, mockArtisanFavService.Object, mockLogger.Object);

        var result = await controller.GetAllProducts();

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value!;
        var itemsProperty = value.GetType().GetProperty("items");
        Assert.NotNull(itemsProperty);

        var items = itemsProperty!.GetValue(value) as IEnumerable<object>;
        Assert.NotNull(items);

        var first = items!.First();
        var sku = first.GetType().GetProperty("Sku")!.GetValue(first) as string;
        Assert.Equal("SKU-OLDER", sku);
    }

    [Fact]
    public async Task GetAllProducts_IncludesPrimaryImageUrlAndArtisanFavStatus()
    {
        var mockProductService = new Mock<IProductService>();
        var mockSkuService = new Mock<ISkuGeneratorService>();
        var mockImageService = new Mock<IProductImageService>();
        var mockArtisanFavService = new Mock<IArtisanFavService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        var sku = "RI-RS-FL-ONM-2024-1";
        mockProductService.Setup(s => s.GetAllProducts()).ReturnsAsync(new List<ProductEntity>
        {
            new()
            {
                SKU = sku,
                PartitionKey = "RI",
                ProductName = "Test Product",
                Price = 100,
                Stock = 5,
                MaterialCode = "RS",
                CollectionCode = "ONM",
                FeatureCodes = "FL",
                YearCode = 2024
            }
        });

        mockImageService.Setup(s => s.GetPrimaryImageUrlForSkuId(sku))
            .ReturnsAsync("https://blob.sas.url/image.jpg");

        mockArtisanFavService.Setup(s => s.GetAll())
            .ReturnsAsync(new[] { sku });

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockImageService.Object, mockArtisanFavService.Object, mockLogger.Object);

        var result = await controller.GetAllProducts();

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value!;
        var itemsProperty = value.GetType().GetProperty("items");
        Assert.NotNull(itemsProperty);

        var items = (itemsProperty!.GetValue(value) as IEnumerable<ProductListItemResponse>)?.ToList();
        Assert.NotNull(items);
        Assert.Single(items);

        var item = items!.First();
        Assert.Equal(sku, item.Sku);
        Assert.Equal("https://blob.sas.url/image.jpg", item.PrimaryImageUrl);
        Assert.True(item.IsArtisanFav);
    }

    [Fact]
    public async Task GetProduct_ReturnsProductResponseWithAllImagesAndArtisanFavFlag()
    {
        var mockProductService = new Mock<IProductService>();
        var mockSkuService = new Mock<ISkuGeneratorService>();
        var mockImageService = new Mock<IProductImageService>();
        var mockArtisanFavService = new Mock<IArtisanFavService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        var sku = "RI-RS-FL-ONM-2024-1";
        mockProductService.Setup(s => s.GetProductBySku(sku)).ReturnsAsync(new ProductEntity
        {
            SKU = sku,
            PartitionKey = "RI",
            ProductName = "Test",
            ProductDescription = "Desc",
            Price = 10,
            DiscountPrice = 8,
            DiscountExpiryDate = DateTimeOffset.UtcNow.AddDays(2),
            Stock = 3,
            MaterialCode = "RS",
            CollectionCode = "ONM",
            FeatureCodes = "FL",
            YearCode = 2024
        });

        mockImageService.Setup(s => s.GetPrimaryImageIdForSkuId(sku))
            .ReturnsAsync("image-1");

        mockImageService.Setup(s => s.GetImageIdsForSkuId(sku))
            .ReturnsAsync(new[] { "image-1", "image-2" });

        mockImageService.Setup(s => s.GetPrimaryImageUrlForSkuId(sku))
            .ReturnsAsync("https://blob.sas.url/image.jpg");

        mockArtisanFavService.Setup(s => s.GetAll())
            .ReturnsAsync(new[] { sku });

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockImageService.Object, mockArtisanFavService.Object, mockLogger.Object);

        var result = await controller.GetProduct(sku);
        
        var ok = Assert.IsType<OkObjectResult>(result);
        var response = ok.Value as ProductResponse;
        Assert.NotNull(response);
        Assert.Equal(sku, response.Sku);
        Assert.NotNull(response.Images);
        Assert.Equal(2, response.Images.Count());
        
        var images = response.Images.ToList();
        Assert.Equal("image-1", images[0].ImageId);
        Assert.True(images[0].IsPrimary);
        Assert.Equal("https://blob.sas.url/image.jpg", images[0].ImageUrl);
        
        Assert.Equal("image-2", images[1].ImageId);
        Assert.False(images[1].IsPrimary);
        Assert.Equal("https://blob.sas.url/image.jpg", images[1].ImageUrl);
        
        Assert.True(response.IsArtisanFav);
    }

    [Fact]
    public async Task GetProduct_ReturnsDiscountPrice_WhenPresent()
    {
        var mockProductService = new Mock<IProductService>();
        var mockSkuService = new Mock<ISkuGeneratorService>();
        var mockImageService = new Mock<IProductImageService>();
        var mockArtisanFavService = new Mock<IArtisanFavService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        var sku = "RI-RS-FL-ONM-2024-1";
        mockProductService.Setup(s => s.GetProductBySku(sku)).ReturnsAsync(new ProductEntity
        {
            SKU = sku,
            PartitionKey = "RI",
            ProductName = "Test",
            ProductDescription = "Desc",
            Price = 10,
            DiscountPrice = 8,
            DiscountExpiryDate = DateTimeOffset.UtcNow.AddDays(2),
            Stock = 3,
            MaterialCode = "RS",
            CollectionCode = "ONM",
            FeatureCodes = "FL",
            YearCode = 2024
        });

        mockImageService.Setup(s => s.GetPrimaryImageUrlForSkuId(sku)).ReturnsAsync((string?)null);
        mockArtisanFavService.Setup(s => s.GetAll()).ReturnsAsync(new List<string>());

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockImageService.Object, mockArtisanFavService.Object, mockLogger.Object);

        var result = await controller.GetProduct(sku);
        var ok = Assert.IsType<OkObjectResult>(result);
        var response = ok.Value as ProductResponse;
        Assert.NotNull(response);
        Assert.Equal(8d, response.DiscountPrice);
    }

    [Fact]
    public async Task GetProduct_ReturnsPriceAsDiscountPrice_WhenDiscountExpired()
    {
        var mockProductService = new Mock<IProductService>();
        var mockSkuService = new Mock<ISkuGeneratorService>();
        var mockImageService = new Mock<IProductImageService>();
        var mockArtisanFavService = new Mock<IArtisanFavService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        var sku = "RI-RS-FL-ONM-2024-1";
        mockProductService.Setup(s => s.GetProductBySku(sku)).ReturnsAsync(new ProductEntity
        {
            SKU = sku,
            PartitionKey = "RI",
            ProductName = "Test",
            ProductDescription = "Desc",
            Price = 10,
            DiscountPrice = 8,
            DiscountExpiryDate = DateTimeOffset.UtcNow.AddDays(-1),
            Stock = 3,
            MaterialCode = "RS",
            CollectionCode = "ONM",
            FeatureCodes = "FL",
            YearCode = 2024
        });

        mockImageService.Setup(s => s.GetPrimaryImageUrlForSkuId(sku)).ReturnsAsync((string?)null);
        mockArtisanFavService.Setup(s => s.GetAll()).ReturnsAsync(new List<string>());

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockImageService.Object, mockArtisanFavService.Object, mockLogger.Object);

        var result = await controller.GetProduct(sku);
        var ok = Assert.IsType<OkObjectResult>(result);
        var response = ok.Value as ProductResponse;
        Assert.NotNull(response);
        Assert.Equal(10d, response.DiscountPrice);
    }


    [Fact]
    public async Task GetProduct_ReturnsNullDiscountPrice_WhenDiscountNotProvided()
    {
        var mockProductService = new Mock<IProductService>();
        var mockSkuService = new Mock<ISkuGeneratorService>();
        var mockImageService = new Mock<IProductImageService>();
        var mockArtisanFavService = new Mock<IArtisanFavService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        var sku = "RI-RS-FL-ONM-2024-1";
        mockProductService.Setup(s => s.GetProductBySku(sku)).ReturnsAsync(new ProductEntity
        {
            SKU = sku,
            PartitionKey = "RI",
            ProductName = "Test",
            ProductDescription = "Desc",
            Price = 10,
            DiscountPrice = null,
            DiscountExpiryDate = null,
            Stock = 3,
            MaterialCode = "RS",
            CollectionCode = "ONM",
            FeatureCodes = "FL",
            YearCode = 2024
        });

        mockImageService.Setup(s => s.GetPrimaryImageUrlForSkuId(sku)).ReturnsAsync((string?)null);
        mockArtisanFavService.Setup(s => s.GetAll()).ReturnsAsync(new List<string>());

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockImageService.Object, mockArtisanFavService.Object, mockLogger.Object);

        var result = await controller.GetProduct(sku);
        var ok = Assert.IsType<OkObjectResult>(result);
        var response = ok.Value as ProductResponse;
        Assert.NotNull(response);
        Assert.Null(response.DiscountPrice);
    }


    [Fact]
    public async Task DeleteProduct_ReturnsOk_OnSuccess()
    {
        var mockProductService = new Mock<IProductService>();
        var mockSkuService = new Mock<ISkuGeneratorService>();
        var mockImageService = new Mock<IProductImageService>();
        var mockArtisanFavService = new Mock<IArtisanFavService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        mockProductService.Setup(s => s.DeleteProduct("RI-RS-FL-ONM-2024-1")).Returns(Task.CompletedTask);

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockImageService.Object, mockArtisanFavService.Object, mockLogger.Object);

        var result = await controller.DeleteProduct("RI-RS-FL-ONM-2024-1");

        var ok = Assert.IsType<OkObjectResult>(result);
        var skuProp = ok.Value!.GetType().GetProperty("sku");
        Assert.NotNull(skuProp);
        Assert.Equal("RI-RS-FL-ONM-2024-1", skuProp!.GetValue(ok.Value));

        mockProductService.Verify(s => s.DeleteProduct("RI-RS-FL-ONM-2024-1"), Times.Once);
    }

    [Fact]
    public async Task SearchProducts_IncludesPrimaryImageUrlAndArtisanFavStatus()
    {
        var mockProductService = new Mock<IProductService>();
        var mockSkuService = new Mock<ISkuGeneratorService>();
        var mockImageService = new Mock<IProductImageService>();
        var mockArtisanFavService = new Mock<IArtisanFavService>();
        var mockLogger = new Mock<ILogger<ProductController>>();

        var sku1 = "RI-RS-FL-ONM-2024-1";
        var sku2 = "RI-RS-FL-ONM-2024-2";

        mockProductService.Setup(s => s.SearchProductsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<ProductEntity>
            {
                new()
                {
                    SKU = sku1,
                    PartitionKey = "RI",
                    ProductName = "Product 1",
                    Price = 100,
                    Stock = 5,
                    MaterialCode = "RS",
                    CollectionCode = "ONM",
                    FeatureCodes = "FL",
                    YearCode = 2024
                },
                new()
                {
                    SKU = sku2,
                    PartitionKey = "RI",
                    ProductName = "Product 2",
                    Price = 150,
                    Stock = 3,
                    MaterialCode = "RS",
                    CollectionCode = "ONM",
                    FeatureCodes = "FL",
                    YearCode = 2024
                }
            });

        mockImageService.Setup(s => s.GetPrimaryImageUrlForSkuId(sku1))
            .ReturnsAsync("https://blob.sas.url/image1.jpg");

        mockImageService.Setup(s => s.GetPrimaryImageUrlForSkuId(sku2))
            .ReturnsAsync("https://blob.sas.url/image2.jpg");

        mockArtisanFavService.Setup(s => s.GetAll())
            .ReturnsAsync(new[] { sku1 });

        var controller = new ProductController(mockProductService.Object, mockSkuService.Object, mockImageService.Object, mockArtisanFavService.Object, mockLogger.Object);

        var filters = new SearchProductsRequest { SelectedCategories = new[] { "RI" } };
        var result = await controller.SearchProducts(filters);

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value!;
        var itemsProperty = value.GetType().GetProperty("items");
        Assert.NotNull(itemsProperty);

        var items = (itemsProperty!.GetValue(value) as IEnumerable<ProductListItemResponse>)?.ToList();
        Assert.NotNull(items);
        Assert.Equal(2, items!.Count);

        var item1 = items![0];
        Assert.Equal(sku1, item1.Sku);
        Assert.Equal("https://blob.sas.url/image1.jpg", item1.PrimaryImageUrl);
        Assert.True(item1.IsArtisanFav);

        var item2 = items![1];
        Assert.Equal(sku2, item2.Sku);
        Assert.Equal("https://blob.sas.url/image2.jpg", item2.PrimaryImageUrl);
        Assert.False(item2.IsArtisanFav);
    }
}