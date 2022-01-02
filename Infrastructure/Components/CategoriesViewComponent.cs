using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Infrastructure.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ShopCartDbContext _dbContext;

        public CategoriesViewComponent(ShopCartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync() =>
           View(await _dbContext.Categories.OrderBy(x => x.Sorting).ToListAsync());
    }
}
