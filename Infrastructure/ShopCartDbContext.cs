using Kupri4.ShopCart.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kupri4.ShopCart.Infrastructure
{
    public class ShopCartDbContext : IdentityDbContext<AppUser>
    {
        public ShopCartDbContext(DbContextOptions<ShopCartDbContext> options)
            : base(options)
        { }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
