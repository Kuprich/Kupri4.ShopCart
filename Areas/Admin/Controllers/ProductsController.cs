using Kupri4.ShopCart.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ShopCartDbContext _dbContext;

        public ProductsController(ShopCartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET admin/products
        public async Task<ViewResult> Index()
        {
            var products = await _dbContext.Products.Include(x => x.Category).ToListAsync();

            return View(products);
        }

        // GET admin/products/create
        public ViewResult Create()
        {
            ViewBag.CategoryId = new SelectList(_dbContext.Categories.OrderBy(x => x.Sorting), "Id", "Name");
            return View();
        }
    }
}
