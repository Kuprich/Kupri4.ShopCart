using System.ComponentModel.DataAnnotations;

namespace Kupri4.ShopCart.Models
{
    public class Page
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public string Content { get; set; }

        public int Sorting { get; set; }
    }
}
