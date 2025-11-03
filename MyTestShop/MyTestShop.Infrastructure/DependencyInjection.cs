using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using MyTestShop.Domain.Abstractions;
using MyTestShop.Infrastructure.Repositories;

namespace MyTestShop.Infrastructure
{
    public static class DependencyInjection
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            // If running on Windows and the developer hasn't changed the connection string,
            // prefer LocalDB so the project "just works" when opened in Visual Studio.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (string.IsNullOrWhiteSpace(connectionString) || connectionString.Contains("localhost", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=MyTestShopDb;Trusted_Connection=True;MultipleActiveResultSets=true";
                }
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("Database connection string is not configured.");
            }

            services.AddDbContext<MyTestShopDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    // Enable resilient SQL connections (retry on transient failures)
                    sqlOptions.EnableRetryOnFailure();

                    // Ensure EF migrations are generated/applied from the same assembly as the DbContext
                    sqlOptions.MigrationsAssembly(typeof(MyTestShopDbContext).Assembly.FullName);
                });
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<MyTestShopDbContext>());
        }
    }
}
