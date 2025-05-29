using System.Collections.Generic;
using System.Linq;

namespace WoodStore.Models
{
    public static class ProductRepos
    {
        private static List<Product> _products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Oak Plank",
                WoodType = "Oak",
                Price = 20m,
                Stock = 50,
                PurchaseCount = 15,
                IsDiscounted = false,
                Description = "Oak product",
                Weight = 10,
                Width = 5,
                Length = 20,
                Height = 2,
                DiscountPercent = 0
            },
            new Product
            {
                Id = 2,
                Name = "Pine Plank",
                WoodType = "Pine",
                Price = 15m,
                Stock = 70,
                PurchaseCount = 10,
                IsDiscounted = true,
                Description = "Pine product",
                Weight = 8,
                Width = 5,
                Length = 18,
                Height = 2,
                DiscountPercent = 10
            },
            new Product
            {
                Id = 3,
                Name = "Cherry Plank",
                WoodType = "Cherry",
                Price = 25m,
                Stock = 40,
                PurchaseCount = 8,
                IsDiscounted = true,
                Description = "Cherry product",
                Weight = 12,
                Width = 5,
                Length = 22,
                Height = 2,
                DiscountPercent = 15
            }
        }; 
       
        // Return a copy of the product list rather than the original list.
        public static List<Product> GetAllProducts() => _products.ToList();

        public static Product GetProductById(int id) =>
            _products.FirstOrDefault(p => p.Id == id);

        public static void AddProduct(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
        }

        public static void UpdateProduct(Product updated)
        {
            var product = _products.FirstOrDefault(p => p.Id == updated.Id);
            if (product != null)
            {
                product.Name = updated.Name;
                product.WoodType = updated.WoodType;
                product.Description = updated.Description;
                product.Price = updated.Price;
                product.Stock = updated.Stock;
                product.PurchaseCount = updated.PurchaseCount;
                product.IsDiscounted = updated.IsDiscounted;
                product.DiscountPercent = updated.DiscountPercent;
                product.Weight = updated.Weight;
                product.Width = updated.Width;
                product.Length = updated.Length;
                product.Height = updated.Height;
            }
        }

        public static void RemoveProduct(int id)
        {
            var product = GetProductById(id);
            if (product != null)
                _products.Remove(product);
        }
    }
}
