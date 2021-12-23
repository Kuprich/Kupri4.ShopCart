using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kupri4.ShopCart.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum lenght is 2")]
        [RegularExpression(@"^[a-zA-Zа-яА-я- ]+$", ErrorMessage = "Only letters are allowed")]
        public string Name { get; set; }

        public string Slug { get; set; }

        public int Sorting { get; set; }
        public IList<Product> Products { get; set; } = new List<Product>();
    }
}
