using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialController:BaseController
{
    public MaterialController(ILogger<MaterialController> logger):base(logger)
    {
            
    }
    [HttpPost]
    [Route("create")]
    public IActionResult CreateMaterial()
    {
        Logger.LogInformation("Test endpoint hit");
        return Ok("MaterialController is working!");
    }
}
