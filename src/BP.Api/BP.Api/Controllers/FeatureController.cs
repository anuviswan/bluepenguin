using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeatureController:BaseController
{
    public FeatureController(ILogger<FeatureController> logger):base(logger)
    {
            
    }
    [HttpPost]
    [Route("create")]
    public IActionResult CreateFeature()
    {
        Logger.LogInformation("Test endpoint hit");
        return Ok("FeatureController is working!");
    }
}
