using System.Text.Json;
using Microsoft.AspNetCore.Http;
using WoodStore.Models;  // Make sure this is your correct namespace for Cart

namespace WoodStore.Util
{
    public static class SessionExtensions
    {
        private const string CartSessionKey = "Cart";

        // Save the cart to the session by serializing it as JSON
        public static void SetCart(this ISession session, Cart cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            session.SetString(CartSessionKey, cartJson);
        }

        // Retrieve the cart from the session.
        // If none exists, create a new Cart, store it, and return it.
        public static Cart GetCart(this ISession session)
        {
            var cartJson = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                var newCart = new Cart();
                session.SetCart(newCart);
                return newCart;
            }
            return JsonSerializer.Deserialize<Cart>(cartJson);
        }
    }
}
