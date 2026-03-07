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
        private readonly IComputerVisionService _computerVisionService;
        private readonly ILogger<ImageSearchController> _logger;

        public ImageSearchController(IComputerVisionService computerVisionService, ILogger<ImageSearchController> logger)
        {
            _computerVisionService = computerVisionService;
            _logger = logger;
        }

        [HttpPost("generate-embeddings")]
        public async Task<IActionResult> GenerateProductEmbeddings([FromQuery] bool force = false)
        {
            try
            {
                var result = await _computerVisionService.GenerateEmbeddingsForAllImagesAsync(force);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
