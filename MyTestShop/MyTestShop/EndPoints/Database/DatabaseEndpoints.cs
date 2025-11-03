using Microsoft.EntityFrameworkCore;
using MyTestShop.Infrastructure;

namespace MyTestShop.EndPoints.Database
{
    public static class DatabaseEndpoints
    {
        public static void MapDatabaseEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/database")
                .WithTags("Database Admin")
                .WithOpenApi();

            group.MapGet("/customers", async (MyTestShopDbContext context) =>
            {
                var customers = await context.Customers.ToListAsync();
                return Results.Ok(customers);
            })
            .WithName("GetAllCustomers")
            .WithSummary("Get all customers from database");

            group.MapGet("/products", async (MyTestShopDbContext context) =>
            {
                var products = await context.Products.ToListAsync();
                return Results.Ok(products);
            })
            .WithName("GetAllProducts")
            .WithSummary("Get all products from database");

            group.MapGet("/orders", async (MyTestShopDbContext context) =>
            {
                var orders = await context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .ToListAsync();
                return Results.Ok(orders);
            })
            .WithName("GetAllOrders")
            .WithSummary("Get all orders with items and product info");

            group.MapGet("/items", async (MyTestShopDbContext context) =>
            {
                var items = await context.Items
                    .Include(i => i.Product)
                    .ToListAsync();
                return Results.Ok(items);
            })
            .WithName("GetAllItems")
            .WithSummary("Get all order items with product info");

            group.MapGet("/stats", async (MyTestShopDbContext context) =>
            {
                var stats = new
                {
                    TotalCustomers = await context.Customers.CountAsync(),
                    TotalProducts = await context.Products.CountAsync(),
                    TotalOrders = await context.Orders.CountAsync(),
                    TotalOrderItems = await context.Items.CountAsync()
                };
                return Results.Ok(stats);
            })
            .WithName("GetDatabaseStats")
            .WithSummary("Get database statistics");
        }
    }
}