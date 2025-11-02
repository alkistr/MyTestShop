using Microsoft.EntityFrameworkCore;
using MyTestShop.Domain.Customers;

namespace MyTestShop.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MyTestShopDbContext _dbContext;
        public CustomerRepository(MyTestShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Customer customer)
        {
            _dbContext.Customers.Add(customer);
        }

        public void Delete(Customer customer)
        {
            _dbContext.Customers.Remove(customer);
        }

        public async Task<Customer> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Customers
                .FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);
        }
    }
    
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        Task<Customer> GetByIdAsync(int id, CancellationToken cancellationToken);
        void Delete(Customer customer);
    }
}
