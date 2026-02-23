using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtisanFavController(IArtisanFavService artisanFavService, ILogger<ArtisanFavController> logger) : BaseController(logger)
{
    [HttpGet("getall")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var artisanFavs = await artisanFavService.GetAll();
            return Ok(artisanFavs);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to fetch artisan favs.");
            return BadRequest(e.Message);
        }
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ArtisanFavRequest request)
    {
        try
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Sku))
            {
                return BadRequest("Invalid request");
            }

            await artisanFavService.Add(request.Sku);
            return Ok();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to create artisan fav for sku {Sku}", request.Sku);
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("delete/{sku}")]
    [Authorize]
    public async Task<IActionResult> Delete(string sku)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                return BadRequest("Invalid sku");
            }

            await artisanFavService.Delete(sku);
            return Ok();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to delete artisan fav for sku {Sku}", sku);
            return BadRequest(e.Message);
        }
    }
}
