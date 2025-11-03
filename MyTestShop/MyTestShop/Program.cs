using MyTestShop.Application;
using MyTestShop.Infrastructure;
using MyTestShop.EndPoints.Customers;
using MyTestShop.EndPoints.Orders;
using MyTestShop.EndPoints.Database;
using MyTestShop.Database;

namespace MyTestShop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddAuthorization();

            // Enable OpenAPI/Swagger generation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.RegisterApplicationServices();
            builder.Services.RegisterInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            // Seed database in development environment
            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<MyTestShopDbContext>();
                    await DatabaseSeeder.SeedDataAsync(context);
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Enable middleware to serve generated Swagger as JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui assets (HTML, JS, CSS, etc.)
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.MapCustomersEndpoints();
            app.MapOrdersEndpoints();
            app.MapDatabaseEndpoints();

            await app.RunAsync();
        }
    }
}
