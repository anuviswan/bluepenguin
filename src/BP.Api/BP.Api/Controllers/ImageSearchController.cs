using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers
{
    public class ImageSearchController : Controller
    {
        [HttpPost]
        [Route("indexall")]
        public IActionResult IndexAll()
        {
            return View();
        }
    }
}
