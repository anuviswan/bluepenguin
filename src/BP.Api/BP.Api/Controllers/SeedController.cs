using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

public class SeedController(IProductController productController): Controller
{
    private IProductController ProductController => productController;
    public IActionResult ExecuteSeed()
    {
        return Ok(SeedProducts());

    }

    private object? SeedProducts()
    {
        throw new NotImplementedException();
    }




}
