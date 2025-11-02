using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTestShop.Domain.Abstractions;
using MyTestShop.Infrastructure.Repositories;

namespace MyTestShop.Infrastructure
{
    public static class DependencyInjection
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("Database") ??
                throw new ArgumentNullException(nameof(configuration));

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

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<MyTestShopDbContext>());
        }
    }
}
