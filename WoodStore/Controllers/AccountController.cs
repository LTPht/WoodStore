using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WoodStore.Models;
using WoodStore.Data;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using WoodStore.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WoodStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AccountController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /Account/Signup
        public IActionResult Signup()
        {
            return View();
        }

        // POST: /Account/Signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(Customer model)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                if (_context.Customers.Any(c => c.Email == model.Email))
                {
                    ModelState.AddModelError("", "Email is already registered.");
                    return View(model);
                }

                // Hash password before storing
                var hasher = new PasswordHasher<Customer>();
                model.PasswordHash = hasher.HashPassword(model, model.PasswordHash);

                // By default, new sign-ups are not admin.
                model.IsAdmin = false;

                _context.Customers.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Email == email);
            if (customer == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }

            // Verify password hash
            var hasher = new PasswordHasher<Customer>();
            var result = hasher.VerifyHashedPassword(customer, customer.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.Username),
                new Claim(ClaimTypes.Email, customer.Email),
                new Claim("IsAdmin", customer.IsAdmin ? "true" : "false")
            };
            System.Diagnostics.Debug.WriteLine($"IsAdmin: {customer.IsAdmin}");

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Verify if the customer is an admin and redirect accordingly.
            if (customer.IsAdmin)
            {
                // Redirect admin users to the admin dashboard.
                return RedirectToAction("Index", "AdminDashboard");
            }
            else
            {
                // Regular users go to the store's homepage.
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: /Account/Profile
        public IActionResult Profile()
        {
            // Retrieve the currently logged in customer's email from the claims.
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            // Retrieve the customer record using the email.
            var customer = _context.Customers.FirstOrDefault(c => c.Email == email);
            if (customer == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ProfileViewModel
            {
                Username = customer.Username,
                Email = customer.Email,
                ProfilePictureUrl = string.IsNullOrEmpty(customer.ProfilePictureUrl)
                                      ? "/images/default-profile.png"
                                      : customer.ProfilePictureUrl
            };

            return View(model);
        }

        // POST: /Account/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            var customer = _context.Customers.FirstOrDefault(c => c.Email == email);
            if (customer == null)
            {
                return RedirectToAction("Login");
            }

            // Update basic information.
            customer.Username = model.Username;
            customer.Email = model.Email;

            // Process password change if provided.
            if (!string.IsNullOrWhiteSpace(model.CurrentPassword) &&
                !string.IsNullOrWhiteSpace(model.NewPassword) &&
                model.NewPassword == model.ConfirmPassword)
            {
                var hasher = new PasswordHasher<Customer>();
                var result = hasher.VerifyHashedPassword(customer, customer.PasswordHash, model.CurrentPassword);
                if (result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                    return View(model);
                }

                customer.PasswordHash = hasher.HashPassword(customer, model.NewPassword);
            }

            if (model.NewProfilePicture != null && model.NewProfilePicture.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profile");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.NewProfilePicture.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.NewProfilePicture.CopyTo(stream);
                }

                customer.ProfilePictureUrl = "/uploads/profile/" + fileName;
            }

            // Save updates.
            _context.Customers.Update(customer);
            _context.SaveChanges();

            TempData["Message"] = "Profile updated successfully.";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
