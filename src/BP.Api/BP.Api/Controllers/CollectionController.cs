using BP.Api.ExtensionMethods;
using BP.Api.Contracts;
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
    public async Task<IActionResult> GetAll()
    {
        Logger.LogInformation("Get All Collections");
        try
        {
            var cols = await _collectionService.GetAllCollections();
            var collections = cols.Select(x => new { Id = x.RowKey, Name = x.Title });
            return Ok(collections);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCollection([FromBody] AddFeatureRequest req)
    {
        try
        {
            if (req == null || string.IsNullOrWhiteSpace(req.FeatureId) || string.IsNullOrWhiteSpace(req.FeatureName))
                return BadRequest("Invalid request");

            await _collectionService.Add(req.FeatureId, req.FeatureName);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}
