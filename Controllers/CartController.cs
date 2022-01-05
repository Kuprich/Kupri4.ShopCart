using Kupri4.ShopCart.Infrastructure;
using Kupri4.ShopCart.Infrastructure.Extensions;
using Kupri4.ShopCart.Models;
using Kupri4.ShopCart.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kupri4.ShopCart.Controllers
{
    public class CartController : Controller
    {

        private readonly ShopCartDbContext _dbContext;

        public CartController(ShopCartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET /Cart
        public IActionResult Index()
        {
            List<CartItem> cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart") ??
                new List<CartItem>();

            CartViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Total)
            };

            return View(cartVM);
        }

        // GET /Cart/Add/2
        public async Task<IActionResult> Add(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ??
                new List<CartItem>();

            var cartItem = cart.Find(x => x.ProductId == product.Id);

            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity++;
            }

            HttpContext.Session.SetJson("Cart", cart);

            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return RedirectToAction(nameof(Index));
            }

            return ViewComponent("SmallCart");

        }

        // GET /Cart/Decrease/2
        public async Task<IActionResult> Decrease(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ??
                new List<CartItem>();

            var cartItem = cart.Find(x => x.ProductId == product.Id);

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }

            HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction(nameof(Index));
        }

        // GET /Cart/Remove/2
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ??
                new List<CartItem>();

            var cartItem = cart.Find(x => x.ProductId == product.Id);

            cart.Remove(cartItem);

            HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction(nameof(Index));
        }

        // GET /Cart/Clear
        public IActionResult Clear(int id)
        {

            HttpContext.Session.Remove("Cart");

            return RedirectToAction(nameof(Index));
        }

    }
}
