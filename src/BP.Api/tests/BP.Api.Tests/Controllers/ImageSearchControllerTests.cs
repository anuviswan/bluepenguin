using BP.Api.Controllers;
using BP.Application.Interfaces.Services;
using BP.Application.Interfaces.Types;
using BP.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BP.Api.Tests.Controllers
{
    public class ImageSearchControllerTests
    {
        private readonly Mock<IComputerVisionService> _computerVisionService;
        private readonly Mock<IProductService> _productService;
        private readonly Mock<IFileUploadService> _fileUploadService;
        private readonly Mock<ILogger<ImageSearchController>> _logger;
        private ImageSearchController _sut;
        public ImageSearchControllerTests()
        {
            _computerVisionService = new Mock<IComputerVisionService>();
            
            _productService = new Mock<IProductService>();
            _fileUploadService = new Mock<IFileUploadService>();
            _logger = new Mock<ILogger<ImageSearchController>>();

            _sut = new ImageSearchController(_computerVisionService.Object,_logger.Object, _productService.Object, _fileUploadService.Object);
        }

        [Fact]
        public async Task FindClosestProducts_ReturnsBadRequest_WhenImageMissing()
        {


            var result = await _sut.FindClosestProducts(null!, 5);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Image file is required.", badRequest.Value);
        }

        [Fact]
        public async Task FindClosestProducts_ReturnsMatchedProducts()
        {
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

            _computerVisionService
                .Setup(s => s.FindClosestProductImagesAsync(It.IsAny<Stream>(), 5))
                .ReturnsAsync(matches);

            _productService
                .Setup(s => s.GetProductBySku("RI-1"))
                .ReturnsAsync(new ProductEntity { SKU = "RI-1", ProductName = "Ring", Price = 100, DiscountPrice = 90 });

            _productService
                .Setup(s => s.GetProductBySku("CU-2"))
                .ReturnsAsync(new ProductEntity { SKU = "CU-2", ProductName = "Cuff", Price = 120, DiscountPrice = null });

            _fileUploadService
                .Setup(s => s.GetBlobUrlAsync("blob-1"))
                .ReturnsAsync("https://cdn/blob-1");

            _fileUploadService
                .Setup(s => s.GetBlobUrlAsync("blob-2"))
                .ReturnsAsync("https://cdn/blob-2");

            var result = await _sut.FindClosestProducts(formFile, 5);

            var ok = Assert.IsType<OkObjectResult>(result);
            var payload = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);
            Assert.Equal(2, payload.Count());
        }
    }
}
