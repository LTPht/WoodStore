using System.Collections.Generic;

namespace WoodStore.Models.ViewModel
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public int TotalStock { get; set; }
        public Dictionary<string, double>? WoodTypePercentages { get; set; }
    }
}