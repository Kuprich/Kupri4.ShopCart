using Microsoft.AspNetCore.Identity;

namespace Kupri4.ShopCart.Models
{
    public class AppUser : IdentityUser
    {
        public string Occupation { get; set; }
    }
}
