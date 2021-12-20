using Kupri4.ShopCart.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Kupri4.ShopCart.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ShopCartDbContext _dbContext;

        public CategoryController(ShopCartDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
