using Kupri4.ShopCart.Models;
using Kupri4.ShopCart.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET /Admin/Roles
        public ViewResult Index() =>
            View(_roleManager.Roles);

        // GET /Admin/Roles/Create
        public ViewResult Create() =>
            View();

        //POST /Admin/Roles/Create/{Role name}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required, MinLength(3)] string name)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Minimum length is 3");
                return View();
            }
            var result = await _roleManager.CreateAsync(new IdentityRole(name.Trim()));

            if (result.Succeeded)
            {
                TempData["Success"] = "The role has been created";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                ModelState.AddModelErrors(result.Errors);
            }

            return View();
        }
        
        // GET /Admin/Roles/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return RedirectToAction(nameof(Index));
            }

            List<AppUser> members = new();
            List<AppUser> nonMembers = new();

            foreach (var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }

            RoleEditViewModel vm = new()
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Members = members,
                NonMembers = nonMembers
            };

            return View(vm);
        }

        // POST /Admin/Roles/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            IdentityRole role = await _roleManager.FindByIdAsync(vm.RoleId);

            if (role == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (vm.AddIds != null && vm.AddIds?.Length != 0)
            {
                foreach (var userId in vm.AddIds)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    var result = await _userManager.AddToRoleAsync(user, role.Name);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelErrors(result.Errors);
                    }
                }
            }
            if (vm.DeleteIds != null && vm.DeleteIds.Length != 0)
            {
                foreach (var userId in vm.DeleteIds)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelErrors(result.Errors);
                    }
                }
            }

            TempData["Success"] = "The role was successfully edited";

            return RedirectToAction(nameof(Index));
        }
    }
}
