using System.ComponentModel.DataAnnotations;

namespace Kupri4.ShopCart.Models.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "User name")]
        [Required, MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
