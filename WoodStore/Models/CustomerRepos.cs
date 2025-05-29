using System;
using System.Collections.Generic;
using System.Linq;

namespace WoodStore.Models
{
    public static class CustomerRepository
    {
        private static List<Customer> _customers = new List<Customer>
        {
            new Customer { Uid = Guid.NewGuid().ToString(), Username = "test1", Email = "test1@example.com", DateCreated = DateTime.Now },
            new Customer { Uid = Guid.NewGuid().ToString(), Username = "test2", Email = "test2@example.com", DateCreated = DateTime.Now }
        };

        public static List<Customer> GetAllCustomers() => _customers;

        public static Customer GetCustomerById(string uid) =>
            _customers.FirstOrDefault(c => c.Uid == uid);

        public static void AddCustomer(Customer customer)
        {
            // Auto generate UID and record the creation date
            customer.Uid = Guid.NewGuid().ToString();
            customer.DateCreated = DateTime.Now;
            _customers.Add(customer);
        }
    }
}