using Microsoft.AspNetCore.Mvc;
using WoodStore.Models;
using WoodStore.Models.ViewModel;

namespace WoodStore.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var products = ProductRepos.GetAllProducts();

            int totalStock = products.Sum(p => p.Stock);

            var woodTypePercentages = products
                .GroupBy(p => p.WoodType)
                .ToDictionary(
                    g => g.Key,
                    g => totalStock > 0 ? g.Sum(p => p.Stock) * 100.0 / totalStock : 0);

            var model = new AdminDashboardViewModel
            {
                TotalStock = totalStock,
                WoodTypePercentages = woodTypePercentages
            };
            ViewBag.Title = "Admin Dashboard";
            return View(model);
        }
    }
}