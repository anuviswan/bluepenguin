using BP.Api.Controllers;
using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BP.Api.Tests.Controllers;

public class ArtisanFavControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsOkWithFavs()
    {
        var service = new Mock<IArtisanFavService>();
        var logger = new Mock<ILogger<ArtisanFavController>>();

        service.Setup(x => x.GetAll()).ReturnsAsync(["sku-1", "sku-2"]);

        var controller = new ArtisanFavController(service.Object, logger.Object);

        var result = await controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        var skus = Assert.IsAssignableFrom<IEnumerable<string>>(ok.Value);
        Assert.Equal(2, skus.Count());
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenRequestIsInvalid()
    {
        var service = new Mock<IArtisanFavService>();
        var logger = new Mock<ILogger<ArtisanFavController>>();

        var controller = new ArtisanFavController(service.Object, logger.Object);

        var result = await controller.Create(new ArtisanFavRequest(string.Empty));

        Assert.IsType<BadRequestObjectResult>(result);
        service.Verify(x => x.Add(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenSkuIsValid()
    {
        var service = new Mock<IArtisanFavService>();
        var logger = new Mock<ILogger<ArtisanFavController>>();

        var controller = new ArtisanFavController(service.Object, logger.Object);

        var result = await controller.Delete("sku-1");

        Assert.IsType<OkResult>(result);
        service.Verify(x => x.Delete("sku-1"), Times.Once);
    }
}
