using System.Threading.Tasks;
using Xunit;
using Moq;
using BP.Api.Controllers;
using BP.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using BP.Api.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using BP.Shared.Types;

namespace BP.Api.Tests.Controllers;

public class FileUploadControllerTests
{
    [Fact]
    public async Task UploadProductImage_ReturnsUrl_OnSuccess()
    {
        var mockImageService = new Mock<IProductImageService>();
        var mockProductService = new Mock<IProductService>();
        var mockLogger = new Mock<ILogger<FileUploadController>>();

        var controller = new FileUploadController(mockImageService.Object, mockProductService.Object, mockLogger.Object);

        var content = "dummy";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var formFile = new FormFile(stream, 0, stream.Length, "file", "dummy.png")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };

        var req = new FileUploadRequest { SkuId = "RI-1", File = formFile };

        mockImageService.Setup(s => s.UploadAsync(It.IsAny<FileUpload>(), It.IsAny<bool>())).ReturnsAsync("http://image.url/test.png");

        var result = await controller.UploadProductImage(req, true);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task MarkPrimaryImage_ReturnsOk_WhenSuccessful()
    {
        var mockImageService = new Mock<IProductImageService>();
        var mockProductService = new Mock<IProductService>();
        var mockLogger = new Mock<ILogger<FileUploadController>>();

        mockImageService
            .Setup(s => s.SetPrimaryImageAsync("RI-1", "IMG-1"))
            .ReturnsAsync(true);

        var controller = new FileUploadController(mockImageService.Object, mockProductService.Object, mockLogger.Object);

        var result = await controller.MarkPrimaryImage("RI-1", "IMG-1");

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task MarkPrimaryImage_ReturnsNotFound_WhenImageDoesNotExist()
    {
        var mockImageService = new Mock<IProductImageService>();
        var mockProductService = new Mock<IProductService>();
        var mockLogger = new Mock<ILogger<FileUploadController>>();

        mockImageService
            .Setup(s => s.SetPrimaryImageAsync("RI-1", "IMG-X"))
            .ReturnsAsync(false);

        var controller = new FileUploadController(mockImageService.Object, mockProductService.Object, mockLogger.Object);

        var result = await controller.MarkPrimaryImage("RI-1", "IMG-X");

        Assert.IsType<NotFoundObjectResult>(result);
    }
}
