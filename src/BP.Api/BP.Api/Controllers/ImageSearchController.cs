using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageSearchController(IComputerVisionService computerVisionService, 
        ILogger<ImageSearchController> logger,
        IProductService productService,
        IFileUploadService fileUploadService) : ControllerBase
    {
        private IComputerVisionService ComputerVisionService => computerVisionService;
        private ILogger<ImageSearchController> Logger => logger;

        private IProductService ProductService => productService;

        private IFileUploadService FileUploadService => fileUploadService;

        [HttpPost("generate-embeddings")]
        public async Task<IActionResult> GenerateProductEmbeddings([FromQuery] bool force = false)
        {
            try
            {
                var result = await ComputerVisionService.GenerateEmbeddingsForAllImagesAsync(force);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }


        [HttpPost("find-closest")]
        [AllowAnonymous]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> FindClosestProducts([FromForm] IFormFile image, [FromQuery] int limit = 5)
        {
            try
            {
                if (image == null || image.Length == 0)
                {
                    return BadRequest("Image file is required.");
                }

                await using var stream = image.OpenReadStream();
                var matches = await ComputerVisionService.FindClosestProductImagesAsync(stream, limit).ConfigureAwait(false);

                var response = new List<ImageSearchResultResponse>();
                foreach (var match in matches)
                {
                    var product = await ProductService.GetProductBySku(match.SkuId).ConfigureAwait(false);
                    if (product == null)
                    {
                        continue;
                    }

                    var imageUrl = await FileUploadService.GetBlobUrlAsync(match.BlobName).ConfigureAwait(false);
                    response.Add(new ImageSearchResultResponse
                    {
                        SkuId = product.SKU,
                        ImageUrl = imageUrl,
                        Price = product.Price,
                        Discount = product.DiscountPrice,
                        ProductName = product.ProductName
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to find closest products by image");
                return BadRequest(ex.Message);
            }
        }
    }
}
