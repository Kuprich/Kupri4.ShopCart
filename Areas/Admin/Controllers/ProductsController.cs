using Kupri4.ShopCart.Infrastructure;
using Kupri4.ShopCart.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class ProductsController : Controller
    {
        private readonly ShopCartDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ShopCartDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET admin/products/
        public async Task<ViewResult> Index(int p)
        {
            if (p == 0) p = 1;

            int pageSize = 3;

            var products = await _dbContext.Products.Include(x => x.Category)
                .Skip(pageSize * (p-1))
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

            string imageName = "noimage.png";
            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media", "products");
                imageName = Guid.NewGuid() + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product != null)
            {
                ViewBag.CategoryId = new SelectList(_dbContext.Categories.OrderBy(x => x.Sorting), "Id", "Name");

                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "media", "products", product.Image ?? "noimage.png");

                using (var stream = System.IO.File.OpenRead(filePath))
                {
                    product.ImageUpload = new FormFile(stream, 0, stream.Length, "ImageUpload", Path.GetFileName(stream.Name))
                    {
                        Headers = new HeaderDictionary()
                    };
                }

                return View(product);
            }

            return NotFound();
        }

        //POST /admin/products/edit/2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            ViewBag.CategoryId = new SelectList(_dbContext.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            product.Slug = Regex.Replace(product.Name.ToLower().Trim(), @"\s+", "-");

            if (await _dbContext.Products.Where(x => x.Id != product.Id).FirstOrDefaultAsync(x => x.Slug == product.Slug) != null)
            {
                ModelState.AddModelError("", " The product already exists");
                return View(product);
            }

            string imageName = "noimage.png";
            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media", "products");
                imageName = Guid.NewGuid() + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
            }

            TempData["Success"] = $"Product \"{product.Name}\' has been edited";

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
