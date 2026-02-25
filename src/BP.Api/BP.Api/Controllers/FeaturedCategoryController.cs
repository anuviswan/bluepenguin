using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeaturedCategoryController(IFeaturedCategoryService featuredCategoryService, ILogger<FeaturedCategoryController> logger) : BaseController(logger)
{
    [HttpGet("getall")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var featuredCategories = await featuredCategoryService.GetAll().ConfigureAwait(false);
            return Ok(featuredCategories);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to retrieve featured categories");
            return BadRequest(e.Message);
        }
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] FeaturedCategoryRequest request)
    {
        try
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Code))
            {
                return BadRequest("Invalid request");
            }

            await featuredCategoryService.Add(request.Code).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to create featured category for code {Code}", request.Code);
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("delete/{code}")]
    [Authorize]
    public async Task<IActionResult> Delete(string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest("Invalid code");
            }

            await featuredCategoryService.Delete(code).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to delete featured category for code {Code}", code);
            return BadRequest(e.Message);
        }
    }
}
