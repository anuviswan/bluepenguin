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
using BP.Shared.Types;
using System.IO;

namespace BP.Api.Tests.Controllers;

public class SeedControllerTests
{
    [Fact]
    public async Task ExecuteSeed_ReturnsCreatedList()
    {
        var mockProductController = new Mock<IProductController>();
        var mockImageService = new Mock<IProductImageService>();

        mockProductController.Setup(p => p.CreateProduct(It.IsAny<CreateProductRequest>()))
            .ReturnsAsync(new OkObjectResult("RI-1"));

        mockImageService.Setup(i => i.UploadAsync(It.IsAny<FileUpload>(), It.IsAny<bool>()))
            .ReturnsAsync("http://image.url/test.png");

        var controller = new SeedController(mockProductController.Object, mockImageService.Object);

        var result = await controller.ExecuteSeed();
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }
}
