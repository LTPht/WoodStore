using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoodStore.Models;

namespace WoodStore.Controllers
{

    public class AdminCustomerController : Controller
    {
        public IActionResult CustomerManager()
        {
            var model = new Customer();
            return View(model);
        }

        // View details for a customer
        public IActionResult Details(string id)
        {
            var customer = CustomerRepository.GetCustomerById(id);
            if (customer == null)
                return NotFound();
            return View(customer);
        }
    }
}