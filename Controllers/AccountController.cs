using Kupri4.ShopCart.Models;
using Kupri4.ShopCart.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager)
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
                return vm.ReturnUrl == "/"
                    ? RedirectToAction(nameof(Details))
                    : Redirect(vm.ReturnUrl);
            }

            ModelState.AddModelError("", "Login failed, wrong credentials");
            return View(vm);
        }

        // GET /Account/Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }

        // GET /Account/Details
        [Authorize]
        public async Task<IActionResult> Details()
        {

            var appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (appUser == null)
            {
                return Redirect("/");
            }

            UserViewModel vm = new(appUser);

            return View(vm);

        }

        // GET /Account/Edit
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (appUser == null)
            {
                return Redirect("/");
            }

            UserViewModel vm = new(appUser);

            return View(vm);
        }

        // POST /Account/Edit
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AppUser appUser = await _userManager.GetUserAsync(HttpContext.User);

            appUser.UserName = vm.UserName;
            appUser.Email = vm.Email;

            if (vm.NewPassword != null)
            {
                PasswordValidator<AppUser> passwordValidator = new();
                IdentityResult validationPassword = await passwordValidator.ValidateAsync(_userManager, appUser, vm.NewPassword);

                if (!validationPassword.Succeeded)
                {
                    ModelState.AddModelErrors(validationPassword.Errors);
                    return View(vm);
                }

                appUser.PasswordHash = _userManager.PasswordHasher.HashPassword(appUser, vm.NewPassword);

            }

            IdentityResult result = await _userManager.UpdateAsync(appUser);

            if (!result.Succeeded)
            {
                ModelState.AddModelErrors(result.Errors);
                return View(vm);
            }

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(appUser, false);

            TempData["Success"] = "Your information has been edited!";

            return RedirectToAction(nameof(Details));

        }
    }
}
