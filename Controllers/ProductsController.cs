using Kupri4.ShopCart.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Controllers
{
    public class ProductsController: Controller
    {
        private readonly ShopCartDbContext _dbContext;

        public ProductsController(ShopCartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET products/1
        public async Task<ViewResult> Index(int p = 1)
        {

            int pageSize = 6;
            var products = await _dbContext.Products
                .OrderBy(x => x.Id)
                .Skip((p - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CategoryName = "All categories";
            ViewBag.PageNumber = p;

            int productsCount = _dbContext.Products.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)productsCount / pageSize);

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
            int pageSize = 6;
            var products = await _dbContext.Products
                .Where(x => x.CategoryId == category.Id)
                .OrderBy(x => x.Id)
                .Skip((p - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CategoryName = category.Name;
            ViewBag.PageNumber = p;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)products.Count() / pageSize);

            return View(nameof(Index), products);
        }
    }
}
