using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Application.Interfaces.SkuAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeaturedCategoryController(IFeaturedCategoryService featuredCategoryService, IProductService productService, IProductImageService productImageService, ILogger<FeaturedCategoryController> logger) : BaseController(logger)
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

    /// <summary>
    /// Gets all featured categories for showcase display. Returns category code, name and the primary image URL of the latest product in the category.
    /// </summary>
    /// <returns>Collection of featured categories with primary image URLs.</returns>
    [HttpGet("getall-for-showcase")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<FeaturedCategoryShowcaseResponse>>> GetAllForShowcase()
    {
        Logger.LogInformation("Get featured categories for showcase");

        try
        {
            var featuredCategoryCodes = (await featuredCategoryService.GetAll().ConfigureAwait(false)).ToList();

            var results = new List<FeaturedCategoryShowcaseResponse>();
            foreach (var categoryCode in featuredCategoryCodes)
            {
                var products = (await productService.GetProductsByCategory(categoryCode).ConfigureAwait(false)).ToList();
                if (!products.Any()) continue;

                var latestProduct = products.FirstOrDefault();
                if (latestProduct == null) continue;

                var blobUrl = await productImageService.GetPrimaryImageUrlForSkuId(latestProduct.SKU).ConfigureAwait(false);

                results.Add(new FeaturedCategoryShowcaseResponse
                {
                    CategoryCode = categoryCode,
                    CategoryName = GetCategoryName(categoryCode),
                    BlobUrl = blobUrl
                });
            }

            return Ok(results);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to get featured categories for showcase");
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

    private static string GetCategoryName(string categoryCode)
    {
        if (Enum.TryParse<Category>(categoryCode, ignoreCase: true, out var category))
        {
            var member = typeof(Category).GetMember(category.ToString()).FirstOrDefault();
            var description = member?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault();

            return description?.Description ?? category.ToString();
        }

        return categoryCode;
    }
}
