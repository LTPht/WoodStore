using System.Collections.Generic;

namespace WoodStore.Models.ViewModel
{
    public class HomeViewModel
    {
        public List<Product>? TopProducts { get; set; } = new List<Product>();
        public List<Product>? DiscountedProducts { get; set; } = new List<Product>();
    }
}