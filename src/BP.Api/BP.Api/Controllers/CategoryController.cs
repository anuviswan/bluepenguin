using BP.Api.Contracts;
using BP.Api.ExtensionMethods;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService, IProductService productService, IFeaturedCategoryService featuredCategoryService, ILogger<CategoryController> logger) : BaseController(logger)
{
    private ICategoryService CategoryService => categoryService;
    private IProductService ProductService => productService;
    private IFeaturedCategoryService FeaturedCategoryService => featuredCategoryService;

    /// <summary>
    /// Get all categories with product count in each.
    /// </summary>
    [HttpGet]
    [Route("getall")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        Logger.LogInformation("Get All Categories");
        try
        {
            var categories = CategoryService.GetAllCategories().ToList();
            var featuredCategoryIds = (await FeaturedCategoryService.GetAll().ConfigureAwait(false)).ToList();
            var response = new List<CategoryResponse>();

            foreach (var category in categories)
            {
                var categoryId = category.ToString();
                var products = await ProductService.GetProductsByCategory(categoryId).ConfigureAwait(false);
                var productCount = products.Count();
                var isFeatured = featuredCategoryIds.Any(fc => fc.Equals(categoryId, StringComparison.OrdinalIgnoreCase));

                response.Add(new CategoryResponse
                {
                    Id = categoryId,
                    Name = category.GetDescription(),
                    ProductCount = productCount,
                    IsFeatured = isFeatured
                });
            }

            return Ok(response);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error getting all categories");
            return BadRequest(e);
        }
    }
}
