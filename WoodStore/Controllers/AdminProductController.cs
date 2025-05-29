using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoodStore.Models;
using WoodStore.Models.ViewModel;

namespace WoodStore.Controllers
{
    public class AdminProductController : Controller
    {
        public IActionResult ProductManager()
        {
            var products = ProductRepos.GetAllProducts();
            var customers = CustomerRepository.GetAllCustomers();

            var model = new AdminDashboardViewModel
            {
                Products = products,
                Customers = customers
            };

            return View(model);
        }

        // GET: Create new product
        public IActionResult Create()
        {
            return View(new Product());
        }

        // POST: Create new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                ProductRepos.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Edit product
        public IActionResult Edit(int id)
        {
            var product = ProductRepos.GetProductById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        // POST: Edit product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                ProductRepos.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Delete confirmation
        public IActionResult Delete(int id)
        {
            var product = ProductRepos.GetProductById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        // POST: Delete product
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ProductRepos.RemoveProduct(id);
            return RedirectToAction("Index");
        }
    }
}
