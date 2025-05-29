using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WoodStore.Models;
using WoodStore.Models.ViewModel;

namespace WoodStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Retrieve product data
            var products = ProductRepos.GetAllProducts();

            // Populate the ViewModel
            var model = new HomeViewModel
            {
                TopProducts = products.OrderByDescending(p => p.PurchaseCount).Take(3).ToList(),
                DiscountedProducts = products.Where(p => p.IsDiscounted).ToList()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
