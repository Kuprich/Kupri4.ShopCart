using System.ComponentModel.DataAnnotations;

namespace Kupri4.ShopCart.Models
{
    public class User
    {
        public User(AppUser appUser)
        {
            UserName = appUser.UserName;
            Email = appUser.Email;
            Password = appUser.PasswordHash;
        }
        public User() { }

        [Required, MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Password { get; set; }
    }
}
