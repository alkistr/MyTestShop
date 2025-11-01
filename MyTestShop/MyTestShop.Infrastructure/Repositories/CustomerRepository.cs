using Microsoft.EntityFrameworkCore;
using MyTestShop.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Remove(Customer customer)
        {
            _dbContext.Customers.Remove(customer);
        }
    }
    
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        void Remove(Customer customer);
    }
}
