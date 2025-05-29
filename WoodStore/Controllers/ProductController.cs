using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WoodStore.Models;
using WoodStore.Models.ViewModel;

namespace WoodStore.Controllers
{
    public class ProductController : Controller
    {
        // GET: /Product/List
        public IActionResult List(string searchTerm, string category, decimal? weight, decimal? price, decimal? width, decimal? length, decimal? height)
        {
            // Get all available products.
            var allProducts = GetAllProducts();

            // Then filter products based on the provided parameters.
            var filteredProducts = FilterProducts(allProducts, searchTerm, category, weight, price, width, length, height);

            // Create the view model that will be passed to the view.
            var model = new ProductListViewModel
            {
                SearchTerm = searchTerm,
                Products = filteredProducts
            };

            return View(model);
        }

        // GET: /Product/Details/{id}
        // Displays detailed product information.
        public IActionResult Details(int id)
        {
            var product = ProductRepos.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        /// <summary>
        /// Returns all products from the repository.
        /// </summary>
        private List<Product> GetAllProducts()
        {
            return ProductRepos.GetAllProducts();
        }

        /// <summary>
        /// Filters the provided list of products based on filter parameters.
        /// </summary>
        private List<Product> FilterProducts(List<Product> products, string searchTerm, string category, decimal? weight, decimal? price, decimal? width, decimal? length, decimal? height)
        {
            // Filter by product name, case insensitive.
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                products = products
                    .Where(p => p.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            // Filter by wood type (category).
            if (!string.IsNullOrWhiteSpace(category))
            {
                products = products
                    .Where(p => p.WoodType.Equals(category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Filter by numeric criteria.
            if (weight.HasValue)
                products = products.Where(p => p.Weight >= weight.Value).ToList();
            if (price.HasValue)
                products = products.Where(p => p.Price >= price.Value).ToList();
            if (width.HasValue)
                products = products.Where(p => p.Width >= width.Value).ToList();
            if (length.HasValue)
                products = products.Where(p => p.Length >= length.Value).ToList();
            if (height.HasValue)
                products = products.Where(p => p.Height >= height.Value).ToList();

            return products;
        }
    }
}
