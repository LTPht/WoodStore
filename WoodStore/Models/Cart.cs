using System.Collections.Generic;
using System.Linq;
using WoodStore.Models;

namespace WoodStore.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        // Calculate the total quantity of products
        public int TotalItems => Items.Sum(i => i.Quantity);

        // Calculate the total price of products in the cart
        public decimal TotalPrice => Items.Sum(i => i.Product.Price * i.Quantity);
    }
}
