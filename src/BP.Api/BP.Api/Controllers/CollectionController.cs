using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollectionController:BaseController
{
    public CollectionController(ILogger<CollectionController> logger):base(logger)
    {
            
    }
    [HttpPost]
    [Route("create")]
    public IActionResult CreateCollection()
    {
        Logger.LogInformation("Test endpoint hit");
        return Ok("CollectionController is working!");
    }
}
