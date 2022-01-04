using System.Collections.Generic;

namespace Kupri4.ShopCart.Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
