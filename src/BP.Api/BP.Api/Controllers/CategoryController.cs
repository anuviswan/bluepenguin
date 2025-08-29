using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController:BaseController
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger):base(logger)
    {
        _categoryService = categoryService;
    }
    [HttpGet]
    [Route("getall")]
    public Task<IActionResult> GetAll()
    {
        Logger.LogInformation("Get All Categories");
        try
        {
            var categories = _categoryService.GetAllCategories().Select(x=>x.ToString());
            return Task.FromResult<IActionResult>(Ok(categories));
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(BadRequest(e));
        }
    }
}
