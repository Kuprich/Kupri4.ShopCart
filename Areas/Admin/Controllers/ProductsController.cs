using Kupri4.ShopCart.Infrastructure;
using Kupri4.ShopCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ProductsController : Controller
    {
        private const string DefaultImageName = "noimage.png";

        private readonly ShopCartDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly string _uploadsDir;

        public ProductsController(ShopCartDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media", "products");
        }

        // GET admin/products/
        public async Task<ViewResult> Index(int p)
        {
            if (p == 0) p = 1;

            int pageSize = 3;

            var products = await _dbContext.Products.Include(x => x.Category)
                .Skip(pageSize * (p - 1))
                .Take(pageSize)
                .ToListAsync();

            ViewBag.PageNumber = p;

            int productsCount = _dbContext.Products.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)productsCount / pageSize);



            return View(products);
        }

        // GET admin/products/create
        public ViewResult Create()
        {
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.OrderBy(x => x.Sorting), "Id", "Name");
            return View();
        }

        //POST /admin/products/create?p=1
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            ViewBag.CategoryId = new SelectList(_dbContext.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            product.Slug = Regex.Replace(product.Name.ToLower().Trim(), @"\s+", "-");

            if (await _dbContext.Pages.FirstOrDefaultAsync(x => x.Slug == product.Slug) != null)
            {
                ModelState.AddModelError("", " The product already exists");
                return View(product);
            }

            string imageName = DefaultImageName;
            if (product.ImageUpload != null)
            {
                imageName = Guid.NewGuid() + product.ImageUpload.FileName;
                string filePath = Path.Combine(_uploadsDir, imageName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
            }

            product.Image = imageName;

            TempData["Success"] = $"Product \"{product}\' has been added";

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET /admin/products/edit
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product != null)
            {
                ViewBag.CategoryId = new SelectList(_dbContext.Categories.OrderBy(x => x.Sorting), "Id", "Name");

                return View(product);
            }

            return NotFound();
        }

        //POST /admin/products/edit/2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            product.Slug = Regex.Replace(product.Name.ToLower().Trim(), @"\s+", "-");

            if (await _dbContext.Products.Where(x => x.Id != product.Id).FirstOrDefaultAsync(x => x.Slug == product.Slug) != null)
            {
                ModelState.AddModelError("", " The product already exists");
                return View(product);
            }

            string imageName = DefaultImageName;
            if (product.ImageUpload != null)
            {

                if (!product.Image.Equals(DefaultImageName))
                {
                    string imgOldPath = Path.Combine(_uploadsDir, product.Image);
                    if (System.IO.File.Exists(imgOldPath))
                    {
                        System.IO.File.Delete(imgOldPath);
                    }
                }

                imageName = Guid.NewGuid() + product.ImageUpload.FileName;
                string filePath = Path.Combine(_uploadsDir, imageName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
            }

            product.Image = imageName;

            TempData["Success"] = $"Product \"{product.Name}\' has been edited";

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET /admin/products/details/4
        public async Task<IActionResult> Details(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            if (product.Image == null)
            {
                product.Image = DefaultImageName;
            }

            return View(product);
        }

        // Get /admin/products/delete/3
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                TempData["Error"] = $"Product does not exist";
                return RedirectToAction(nameof(Index));
            }

            if (product.Image != null && product.Image != DefaultImageName)
            {
                string filePath = Path.Combine(_uploadsDir, product.Image);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            TempData["Success"] = $"Product \"{product.Name}\' has been deleted";

            return RedirectToAction(nameof(Index));
        }
    }
}
