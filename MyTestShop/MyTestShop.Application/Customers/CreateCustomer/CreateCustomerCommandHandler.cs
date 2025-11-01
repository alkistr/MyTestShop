using MediatR;
using MyTestShop.Domain.Abstractions;
using MyTestShop.Domain.Customers;
using MyTestShop.Domain.Orders;
using MyTestShop.Infrastructure.Repositories;

namespace MyTestShop.Application.Customers.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Simulate customer creation
            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                PostalCode = request.PostalCode,
                Orders = new List<Order>()
            };

            _customerRepository.Add(customer);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0 ? Result.Success(customer.ToString()) : Result.Failure("Failed to create customer.");
        }
    }
}
