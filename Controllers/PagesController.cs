using Kupri4.ShopCart.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Controllers
{
    public class PagesController : Controller
    {
        private readonly ShopCartDbContext _dbContext;

        public PagesController(ShopCartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Pages(string slug)
        {
            if (slug == null)
            {
                return View(await _dbContext.Pages.FirstOrDefaultAsync(x => x.Slug == "home"));
            }

            var page = await _dbContext.Pages.FirstOrDefaultAsync(x => x.Slug == slug);

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }
    }
}
