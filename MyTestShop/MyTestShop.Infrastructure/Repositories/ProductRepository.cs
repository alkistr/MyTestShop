using Microsoft.EntityFrameworkCore;
using MyTestShop.Domain.Products;

namespace MyTestShop.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyTestShopDbContext _dbContext;
        public ProductRepository(MyTestShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Dictionary<int, Product>> LoadProductsAsync()
        {
            return await _dbContext.Products.ToDictionaryAsync(p => p.Id, p => p);
        }
    }

    public interface IProductRepository
    {
        Task<Dictionary<int, Product>> LoadProductsAsync();
    }
}
