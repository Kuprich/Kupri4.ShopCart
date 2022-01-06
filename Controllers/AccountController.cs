using Kupri4.ShopCart.Models;
using Kupri4.ShopCart.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [AllowAnonymous]
        // GET /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        //POST /Account/Register
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserVm userVm)
        {
            if (!ModelState.IsValid)
            {
                return View(userVm);
            }

            AppUser appUser = new()
            {
                UserName = userVm.UserName,
                Email = userVm.Email
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, userVm.Password);

            if (!result.Succeeded)
            {

                ModelState.AddModelErrors(result.Errors);

                return View(userVm);
            }

            return RedirectToAction("Login");
        }
    }
}
