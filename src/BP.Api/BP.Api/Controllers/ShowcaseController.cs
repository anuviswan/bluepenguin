using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShowcaseController : BaseController
{
    private readonly IShowcaseService _showcaseService;

    public ShowcaseController(IShowcaseService showcaseService, ILogger<ShowcaseController> logger) : base(logger)
    {
        _showcaseService = showcaseService;
    }

    [HttpGet]
    [Route("GetTopCategories")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTopCategories(int count = 4)
    {
        Logger.LogInformation("Get top categories with count {Count}", count);

        try
        {
            var categories = await _showcaseService.GetTopCategories(count);
            return Ok(categories);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to get top categories");
            return BadRequest(e);
        }
    }

    [HttpGet]
    [Route("GetTopDiscounts")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTopDiscounts(int count = 4)
    {
        Logger.LogInformation("Get top discounts with count {Count}", count);

        try
        {
            var discounts = await _showcaseService.GetTopDiscounts(count);
            return Ok(discounts);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to get top discounts");
            return BadRequest(e);
        }
    }


    [HttpGet]
    [Route("GetTopCollections")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTopCollections(int count = 4)
    {
        Logger.LogInformation("Get top collections with count {Count}", count);

        try
        {
            var collections = await _showcaseService.GetTopCollections(count);
            return Ok(collections);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to get top collections");
            return BadRequest(e);
        }
    }

}
