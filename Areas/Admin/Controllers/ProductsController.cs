using Kupri4.ShopCart.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Kupri4.ShopCart.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShopCartDbContext _dbContext;

        public ProductsController(ShopCartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET admin/products
        public ViewResult Index() => View();
    }
}
