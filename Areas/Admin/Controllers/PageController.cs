using Microsoft.AspNetCore.Mvc;

namespace Kupri4.ShopCart.Areas.Admin.Controllers
{
    public class PageController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
