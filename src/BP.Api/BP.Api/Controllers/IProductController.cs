using BP.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers
{
    public interface IProductController
    {
        Task<IActionResult> CreateProduct([FromBody] CreateProductRequest product);
        Task<IActionResult> GetAllProducts();
        Task<IActionResult> GetProduct([FromQuery] string sku);
        Task<IActionResult> SearchProducts([FromBody] SearchProductsRequest filters);
    }
}