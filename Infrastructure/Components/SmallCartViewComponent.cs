using Kupri4.ShopCart.Infrastructure.Extensions;
using Kupri4.ShopCart.Models;
using Kupri4.ShopCart.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Kupri4.ShopCart.Infrastructure.Components
{
    public class SmallCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            SmallCartViewModel vm;

            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (cart != null && cart.Count != 0)
            {
                vm = new()
                {
                    NumberOfItems = cart.Sum(x => x.Quantity),
                    TotalAmount = cart.Sum(x => x.Total)
                };
            }
            else
            {
                vm = new();
            }
            return View(vm); 
        }
    }
}
