using Kupri4.ShopCart.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ViewResult> Create()
        {
            ViewBag.CategirieeList = new SelectList();
        }
    }
}
