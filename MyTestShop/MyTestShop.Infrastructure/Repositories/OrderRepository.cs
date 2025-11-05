using Microsoft.EntityFrameworkCore;
using MyTestShop.Domain.Orders;

namespace MyTestShop.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MyTestShopDbContext _dbContext;

        public OrderRepository(MyTestShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }
    }

    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken);
    }
}
