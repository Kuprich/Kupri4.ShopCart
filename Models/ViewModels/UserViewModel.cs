using System.ComponentModel.DataAnnotations;

namespace Kupri4.ShopCart.Models.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        { }

        public UserViewModel(AppUser user)
        {
            UserName = user.UserName;
            Email = user.Email;
            Password = user.PasswordHash;
        }

        [Display(Name = "User name")]
        [Required, MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string ConfirmNewPassword { get; set; }
    }
}
