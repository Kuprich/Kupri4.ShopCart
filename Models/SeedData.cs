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

                await dbContext.Pages.AddRangeAsync(pages);
                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.Products.AddRangeAsync(products);

                await dbContext.SaveChangesAsync();

            }
        }

        private static Page[] pages = new[]
        {
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
                Slug = "about-us",
                Content = "about-us page",
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
        };
        private static Category[] categories = new[]
        {
            new Category
            {
                Name = "Notebooks",
                Slug = "notebooks",
                Sorting = 0
            },

            new Category
            {
                Name = "Smartphones",
                Slug = "smartphones",
                Sorting = 1
            },

            new Category
            {
                Name = "Hardware components",
                Slug = "hardware-components",
                Sorting = 2
            },

            new Category
            {
                Name = "Home Appliance",
                Slug = "home-appliance",
                Sorting = 3
            },

        };
        private static Product[] products = new[]
        {
            new Product
            {
                Name = "Asus A555li",
                Slug = "asus-a-555li",
                Category = categories[0],
                Description = "Asus notebook (gray)"
            },

            new Product
            {
                Name = "Dell vostro",
                Slug = "dell-vostro",
                Category = categories[0],
                Description = "Dell Vostro 3400 (black)"
            },
        };

    }
}
