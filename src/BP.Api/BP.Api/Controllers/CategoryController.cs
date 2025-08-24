using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController:BaseController
{
    public CategoryController(ILogger<CategoryController> logger):base(logger)
    {
            
    }

    [HttpPost]
    [Route("create")]
    public IActionResult CreateCategory()
    {
        Logger.LogInformation("Test endpoint hit");
        return Ok("CategoryController is working!");
    }
}
