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
                .FindAsync(orderId, cancellationToken);
        }
    }

    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken);
    }
}
