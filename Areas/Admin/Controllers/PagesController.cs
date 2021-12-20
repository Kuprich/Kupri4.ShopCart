using Kupri4.ShopCart.Infrastructure;
using Kupri4.ShopCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly ShopCartDbContext _dbContext;

        public PagesController(ShopCartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET /admin/pages/
        public async Task<IActionResult> Index()
        {
            var pagesList = await _dbContext.Pages.OrderBy(x => x.Sorting).ToListAsync();
            return View(pagesList);
        }

        //GET /admin/pages/details/3
        public async Task<IActionResult> Details(int id)
        {
            var page = await _dbContext.Pages.FirstOrDefaultAsync(x => x.Id == id);

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        //GET /admin/pages/create
        public ViewResult Create() =>
            View();

        //POST /admin/pages/create
        [HttpPost]
        public async Task<IActionResult> Create(Page page)
        {
            if (!ModelState.IsValid)
            {
                return View(page);
            }

            page.Slug = Regex.Replace(page.Title.ToLower().Trim(), @"\s+", "-");

            if (await _dbContext.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug) != null)
            {
                ModelState.AddModelError("", " The page already exists");
                return View(page);
            }

            page.Sorting = 100;

            TempData["Success"] = $"Page \"{page.Title}\' has been added"; 

            _dbContext.Pages.Add(page);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET /admin/pages/edit/2
        public async Task<IActionResult> Edit(int id)
        {
            var page = await _dbContext.Pages.FirstOrDefaultAsync(x => x.Id == id);

            if (page != null)
            {
                return View(page);
            }

            return NotFound();
        }

        //POST /admin/pages/edit/2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (!ModelState.IsValid)
            {
                return View(page);
            }

            page.Slug = page.Id == 1
                ? "home"
                : Regex.Replace(page.Title.ToLower().Trim(), @"\s+", "-");


            if (await _dbContext.Pages.Where(x => x.Id != page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug) != null)
            {
                ModelState.AddModelError("", " The page already exists");
                return View(page);
            }

            TempData["Success"] = $"Page \"{page.Title}\' has been edited";

            _dbContext.Pages.Update(page);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Get /admin/pages/delte/3
        public async Task<IActionResult> Delete(int id)
        {
            var page = await _dbContext.Pages.FirstOrDefaultAsync(x => x.Id == id);

            if (page == null)
            {
                TempData["Error"] = $"Page does not exist";
                return RedirectToAction(nameof(Index));
            }

            _dbContext.Pages.Remove(page);
            await _dbContext.SaveChangesAsync();

            TempData["Success"] = $"Page \"{page.Title}\' has been deleted";

            return RedirectToAction(nameof(Index));
        }

        //POST /admin/pages/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (int pageId in id)
            {
                Page page = await _dbContext.Pages.FindAsync(pageId);
                if (page != null)
                {
                    page.Sorting = count++;
                    await _dbContext.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));

        }
    }
}
