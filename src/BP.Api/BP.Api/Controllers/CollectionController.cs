using BP.Api.Contracts;
using BP.Api.ExtensionMethods;
using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<IActionResult> CreateCollection([FromBody] AddCollectionRequest req)
    {
        try
        {
            if (req == null || string.IsNullOrWhiteSpace(req.CollectionId) || string.IsNullOrWhiteSpace(req.CollectionName))
                return BadRequest("Invalid request");

            await _collectionService.Add(req.CollectionId, req.CollectionName);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateCollection([FromBody] AddCollectionRequest req)
    {
        try
        {
            if (req == null || string.IsNullOrWhiteSpace(req.CollectionId))
                return BadRequest("Invalid request");

            await _collectionService.Update(req.CollectionId,req.CollectionName);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}
