using Kupri4.ShopCart.Models;
using Kupri4.ShopCart.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        //POST /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserViewModel userVm)
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

        // GET /Account/Login
        public IActionResult Login(string returnUrl)
        {
            LoginViewModel vm = new()
            {
                ReturnUrl = returnUrl
            };

            return View(vm);
        }

        // POST /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AppUser appUser = await _userManager.FindByEmailAsync(vm.Email);

            if (appUser == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(vm);
            }

            await _signInManager.SignOutAsync();

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser, vm.Password, false, false);

            if (result.Succeeded)
            {
                return Redirect(vm.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Login failed, wrong credentials");
            return View(vm);
        }

        // GET /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }

    }
}
