using Microsoft.AspNetCore.Mvc;
using WoodStore.Models;
using WoodStore.Util;
using System.Linq;

namespace WoodStore.Controllers
{
    public class CartController : Controller
    {
        // Display the current cart.
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetCart();
            return View(cart);
        }

        // Add a product to the cart with a specified quantity.
        [HttpPost]
        public IActionResult Add(int productId, int quantity = 1)
        {
            var product = ProductRepos.GetProductById(productId);
            if (product == null)
                return NotFound();

            var cart = HttpContext.Session.GetCart();

            // Check if the product is already in the cart.
            var existingItem = cart.Items.FirstOrDefault(item => item.Product.Id == productId);
            if (existingItem != null)
            {
                // Increase quantity if already exists.
                existingItem.Quantity += quantity;
            }
            else
            {
                // Create a new CartItem. Using productId as the CartItem's Id for simplicity.
                cart.Items.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    Id = productId
                });
            }

            HttpContext.Session.SetCart(cart);
            return RedirectToAction("Index");
        }

        // Update the quantity of a product in the cart.
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int change)
        {
            var cart = HttpContext.Session.GetCart();
            var item = cart.Items.FirstOrDefault(i => i.Product.Id == productId);

            if (item != null)
            {
                item.Quantity += change;
                if (item.Quantity <= 0)
                {
                    // Remove the item if quantity falls to zero or below.
                    cart.Items.Remove(item);
                }
            }

            HttpContext.Session.SetCart(cart);
            return RedirectToAction("Index");
        }

        // Remove an item entirely from the cart.
        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var cart = HttpContext.Session.GetCart();
            var item = cart.Items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                cart.Items.Remove(item);
            }
            HttpContext.Session.SetCart(cart);
            return RedirectToAction("Index");
        }
    }
}
