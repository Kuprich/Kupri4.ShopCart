using Kupri4.ShopCart.Infrastructure;
using Kupri4.ShopCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Controllers
{
    public class ProductsController: Controller
    {
        private readonly ShopCartDbContext _dbContext;
        private const int PageSize = 2;

        public ProductsController(ShopCartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET products/1
        public async Task<ViewResult> Index(int p = 1)
        {

            var products = await _dbContext.Products
                .OrderBy(x => x.Id)
                .Skip((p - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.PageNumber = p;
            string controller = HttpContext.Request.RouteValues["controller"].ToString();
            ViewBag.PageTarget = $"/{controller}";

            int productsCount = _dbContext.Products.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)productsCount / PageSize);

            return View(products);
        }

        // GET products/smartphones/2
        public async Task<IActionResult> ProductsByCategory(string categorySlug, int p = 1)
        {
            if (categorySlug == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Slug == categorySlug.ToLower());

            if (category == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var allProducts = await _dbContext.Products.ToListAsync();

            var products = await _dbContext.Products
                .Where(x => x.CategoryId == category.Id)
                .OrderBy(x => x.Id)
                .Skip((p - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.CategoryName = category.Name;
            ViewBag.PageNumber = p;

            string controller = HttpContext.Request.RouteValues["controller"].ToString();
            ViewBag.PageTarget = $"/{controller}/{category.Slug}";

            int productsCount = _dbContext.Products.Where(x => x.CategoryId == category.Id).Count();
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)productsCount / PageSize);

            return View(nameof(Index), products);
        }
    }
}
