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
    private readonly IProductService _productService;

    public CollectionController(ICollectionService collectionService, IProductService productService, ILogger<CollectionController> logger) : base(logger)
    {
        _collectionService = collectionService;
        _productService = productService;
    }

    [HttpGet]
    [Route("getall")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        Logger.LogInformation("Get All Collections");
        try
        {
            var cols = (await _collectionService.GetAllCollections().ConfigureAwait(false)).ToList();
            var products = (await _productService.GetAllProducts().ConfigureAwait(false)).ToList();

            var collections = cols.Select(x =>
            {
                var collectionId = x.RowKey;
                var count = products.Count(p =>
                    !string.IsNullOrWhiteSpace(p.CollectionCode) &&
                    p.CollectionCode.Equals(collectionId, StringComparison.OrdinalIgnoreCase));

                return new { Id = x.RowKey, Name = x.Title, ItemCount = count };
            });

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

            await _collectionService.Add(req.CollectionId, req.CollectionName).ConfigureAwait(false);
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

            await _collectionService.Update(req.CollectionId,req.CollectionName).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}
