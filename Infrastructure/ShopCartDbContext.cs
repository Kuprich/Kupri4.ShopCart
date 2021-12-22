using Kupri4.ShopCart.Models;
using Microsoft.EntityFrameworkCore;

namespace Kupri4.ShopCart.Infrastructure
{
    public class ShopCartDbContext : DbContext
    {
        public ShopCartDbContext(DbContextOptions<ShopCartDbContext> options)
            : base(options)
        { }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
