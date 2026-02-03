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
        var mockService = new Mock<IProductImageService>();
        var mockLogger = new Mock<ILogger<FileUploadController>>();

        var controller = new FileUploadController(mockService.Object, mockLogger.Object);

        var content = "dummy";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var formFile = new FormFile(stream, 0, stream.Length, "file", "dummy.png")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };

        var req = new FileUploadRequest { SkuId = "RI-1", File = formFile };

        mockService.Setup(s => s.UploadAsync(It.IsAny<FileUpload>(), It.IsAny<bool>())).ReturnsAsync("http://image.url/test.png");

        var result = await controller.UploadProductImage(req, true);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }
}
