using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kupri4.ShopCart.Models.ViewModels
{
    public class RoleEditViewModel
    {
        public string RoleId { get; set; }
        [Required, MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string RoleName { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
        public string[] AddIds { get; set; }
        public string[] DeleteIds { get; set; }

    }
}
