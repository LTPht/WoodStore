using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WoodStore.Models;

namespace WoodStore.Data
{
    public static class DbSeeder
    {
        public static void SeedAdmin(WebApplication app)
        {
            // Create a service scope to retrieve required services.
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Ensure the database is created.
                context.Database.EnsureCreated();

                // Check if any admin exists; if not, create one.
                if (!context.Customers.Any(c => c.IsAdmin))
                {
                    var admin = new Customer
                    {
                        Username = "admin",
                        Email = "admin@woodstore.com",
                        IsAdmin = true
                    };

                    // Use PasswordHasher to generate a valid password hash.
                    var passwordHasher = new PasswordHasher<Customer>();
                    admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin123!"); // Use your desired admin password

                    context.Customers.Add(admin);
                    context.SaveChanges();
                }
            }
        }
    }
}
