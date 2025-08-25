using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController:BaseController
{
    public ProductController(ILogger<ProductController> logger):base(logger)
    {
            
    }

    [HttpPost]
    [Route("create")]
    public IActionResult CreateProduct()
    {
        Logger.LogInformation("Test endpoint hit");
        return Ok("ProductController is working!");
    }
}
