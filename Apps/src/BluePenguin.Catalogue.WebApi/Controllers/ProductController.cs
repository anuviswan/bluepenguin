using Microsoft.AspNetCore.Mvc;

namespace BluePenguin.Catalogue.WebApi.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
