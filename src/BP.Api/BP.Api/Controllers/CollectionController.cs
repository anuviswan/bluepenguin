using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollectionController:BaseController
{
    private readonly ICategoryService _categoryService;
    public CollectionController(ICategoryService categoryService, ILogger<CollectionController> logger):base(logger)
    {
        _categoryService = categoryService;
    }
    [HttpGet]
    [Route("getall")]
    public Task<IActionResult> GetAll()
    {
        Logger.LogInformation("Get All Collections");
        try
        {
            var collections = _categoryService.GetAllCategories();
            return Task.FromResult<IActionResult>(Ok(collections));
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(BadRequest(e));
        }
    }
}
