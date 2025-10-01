using BP.Api.ExtensionMethods;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollectionController : BaseController
{
    private readonly ICollectionService _collectionService;
    public CollectionController(ICollectionService collectionService, ILogger<CollectionController> logger) : base(logger)
    {
        _collectionService = collectionService;
    }
    [HttpGet]
    [Route("getall")]
    public Task<IActionResult> GetAll()
    {
        Logger.LogInformation("Get All Collections");
        try
        {
            var collections = _collectionService.GetAllCollections().Select(x => new { Id = x.ToString(), Name = x.GetDescription() });
            return Task.FromResult<IActionResult>(Ok(collections));
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(BadRequest(e));
        }
    }
}
