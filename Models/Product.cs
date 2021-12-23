﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kupri4.ShopCart.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        public string Name { get; set; }

        public string Slug { get; set; }

        [MinLength(5, ErrorMessage = "Minimum length is 5")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public Category Category { get; set; }

        [NotMapped]
        public IFormFile ImageUpload { get; set; }
    }
}
