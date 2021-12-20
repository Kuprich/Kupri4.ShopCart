using System.ComponentModel.DataAnnotations;

namespace Kupri4.ShopCart.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(2, ErrorMessage = "Minimum lenght is 2")]
        [RegularExpression(@"^[a-zA-Zа-яА-я]+$", ErrorMessage = "Only letters are allowed")]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }

        public int Sorting { get; set; }
    }
}
