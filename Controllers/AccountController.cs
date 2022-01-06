using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kupri4.ShopCart.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        // GET /Account/Register
        public IActionResult Register()
        {
            return View();
        }
    }
}
