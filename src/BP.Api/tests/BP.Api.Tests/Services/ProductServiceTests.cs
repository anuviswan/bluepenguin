using BP.Application.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;
using BP.Application.Interfaces.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BP.Api.Tests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task DeleteProduct_DeletesProductImagesSectionEntriesAndProduct()
    {
        var productRepository = new Mock<IProductRepository>();
        var sectionProductRepository = new Mock<ISectionProductRepository>();
        var productImageRepository = new Mock<IProductImageRepository>();
        var fileUploadService = new Mock<IFileUploadService>();

        var sku = "RI-RS-FL-ONM-2024-1";
        var product = new ProductEntity { PartitionKey = "RI", RowKey = sku, SKU = sku };
        var images = new List<ProductImageEntity>
        {
            new() { PartitionKey = sku, RowKey = "img-1", BlobName = "products/RI-RS-FL-ONM-2024-1/img-1.png", ContentType = "image/png" },
            new() { PartitionKey = sku, RowKey = "img-2", BlobName = "products/RI-RS-FL-ONM-2024-1/img-2.png", ContentType = "image/png" }
        };

        productRepository.Setup(x => x.GetById("RI", sku)).ReturnsAsync(product);
        productImageRepository.Setup(x => x.GetProductImagesBySku(sku)).ReturnsAsync(images);
        fileUploadService.Setup(x => x.DeleteByBlobNameAsync(It.IsAny<string>())).ReturnsAsync(true);
        productImageRepository.Setup(x => x.DeleteProductImage(sku, It.IsAny<string>())).ReturnsAsync(true);

        var service = new ProductService(productRepository.Object, sectionProductRepository.Object, productImageRepository.Object, fileUploadService.Object);

        await service.DeleteProduct(sku);

        fileUploadService.Verify(x => x.DeleteByBlobNameAsync("products/RI-RS-FL-ONM-2024-1/img-1.png"), Times.Once);
        fileUploadService.Verify(x => x.DeleteByBlobNameAsync("products/RI-RS-FL-ONM-2024-1/img-2.png"), Times.Once);
        productImageRepository.Verify(x => x.DeleteProductImage(sku, "img-1"), Times.Once);
        productImageRepository.Verify(x => x.DeleteProductImage(sku, "img-2"), Times.Once);
        sectionProductRepository.Verify(x => x.DeleteByRowKey(sku), Times.Once);
        productRepository.Verify(x => x.Delete(product), Times.Once);
    }
}
