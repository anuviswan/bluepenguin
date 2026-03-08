using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageSearchController : ControllerBase
    {
        private readonly IProductImageService _productImageService;
        private readonly IProductService _productService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<ImageSearchController> _logger;

        public ImageSearchController(
            IProductImageService productImageService,
            IProductService productService,
            IFileUploadService fileUploadService,
            ILogger<ImageSearchController> logger)
        {
            _productImageService = productImageService;
            _productService = productService;
            _fileUploadService = fileUploadService;
            _logger = logger;
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
                var matches = await _productImageService.FindClosestProductImagesAsync(stream, limit).ConfigureAwait(false);

                var response = new List<ImageSearchResultResponse>();
                foreach (var match in matches)
                {
                    var product = await _productService.GetProductBySku(match.SkuId).ConfigureAwait(false);
                    if (product == null)
                    {
                        continue;
                    }

                    var imageUrl = await _fileUploadService.GetBlobUrlAsync(match.BlobName).ConfigureAwait(false);
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
                _logger.LogError(ex, "Failed to find closest products by image");
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("generate-embeddings")]
        [Authorize]
        public async Task<IActionResult> GenerateProductEmbeddings([FromQuery] bool force = false)
        {
            try
            {
                var result = await _productImageService.GenerateEmbeddingsForAllImagesAsync(force).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
