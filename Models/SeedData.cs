using Kupri4.ShopCart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Models
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var dbContext = new ShopCartDbContext(serviceProvider.GetRequiredService<DbContextOptions<ShopCartDbContext>>()))
            {
                if (await dbContext.Pages.AnyAsync())
                {
                    return;
                }

                await dbContext.Pages.AddRangeAsync(

                    new Page
                    {
                        Title = "Home",
                        Slug = "home",
                        Content = "Home page",
                        Sorting = 0 
                    },

                    new Page
                    {
                        Title = "About Us",
                        Slug = "abuot-us",
                        Content = "abuot-us page",
                        Sorting = 100,
                    },

                    new Page
                    {
                        Title = "Services",
                        Slug = "services",
                        Content = "serivices page",
                        Sorting = 100,
                    },

                    new Page
                    {
                        Title = "Contact",
                        Slug = "contact",
                        Content = "contact page",
                        Sorting = 100,
                    }
                );

                await dbContext.SaveChangesAsync();

            }
        }
    }
}
