using System.ComponentModel.DataAnnotations;

namespace Kupri4.ShopCart.Models
{
    public class Page
    {
        public int Id { get; set; }

        [Required, MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string Title { get; set; }

        public string Slug { get; set; }

        [Required, MinLength(5, ErrorMessage = "Minimum length is 5")]
        public string Content { get; set; }

        public int Sorting { get; set; }
    }
}
