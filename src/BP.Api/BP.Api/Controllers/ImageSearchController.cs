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
        private readonly ILogger<ImageSearchController> _logger;

        public ImageSearchController(IProductImageService productImageService, ILogger<ImageSearchController> logger)
        {
            _productImageService = productImageService;
            _logger = logger;
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
