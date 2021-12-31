using Kupri4.ShopCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Infrastructure.Components
{
    public class MainMenuViewComponent : ViewComponent
    {

        private readonly ShopCartDbContext _dbContext;

        public MainMenuViewComponent(ShopCartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync() => 
            View(await _dbContext.Pages.OrderBy(x => x.Sorting).ToListAsync());

    }
}
