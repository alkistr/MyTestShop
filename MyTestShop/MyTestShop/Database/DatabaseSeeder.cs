using Microsoft.EntityFrameworkCore;
using MyTestShop.Domain.Customers;
using MyTestShop.Domain.Products;
using MyTestShop.Infrastructure;

namespace MyTestShop.Database
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDataAsync(MyTestShopDbContext context)
        {
            // Apply migrations (preferred) so the DB schema is up-to-date when opened in Visual Studio
            await context.Database.MigrateAsync();

            // Check if data already exists
            if (await context.Customers.AnyAsync() || await context.Products.AnyAsync())
            {
                return; // Database has been seeded
            }

            // Add sample customers
            var customers = new[]
            {
                new Customer { FirstName = "John", LastName = "Doe", Address = "123 Main St", PostalCode = "12345" },
                new Customer { FirstName = "Jane", LastName = "Smith", Address = "456 Oak Ave", PostalCode = "67890" },
                new Customer { FirstName = "Mike", LastName = "Johnson", Address = "789 Pine Rd", PostalCode = "11111" }
            };

            context.Customers.AddRange(customers);

            // Add sample products
            var products = new[]
            {
                new Product { Name = "Laptop Computer", Price = 999.99m },
                new Product { Name = "Wireless Mouse", Price = 29.99m },
                new Product { Name = "Mechanical Keyboard", Price = 149.99m },
                new Product { Name = "Monitor 24\"", Price = 299.99m },
                new Product { Name = "USB Cable", Price = 9.99m }
            };

            context.Products.AddRange(products);

            // Save changes
            await context.SaveChangesAsync();
        }
    }
}