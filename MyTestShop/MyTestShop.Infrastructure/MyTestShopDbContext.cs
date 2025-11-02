using Microsoft.EntityFrameworkCore;
using MyTestShop.Domain.Abstractions;
using MyTestShop.Domain.Customers;
using MyTestShop.Domain.Items;
using MyTestShop.Domain.Products;

namespace MyTestShop.Infrastructure
{
    public class MyTestShopDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }

        public MyTestShopDbContext(DbContextOptions<MyTestShopDbContext> options) : base(options)
        {
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                int result = await base.SaveChangesAsync(cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not shown here)
                return 0;
            }
        }
    }
}
