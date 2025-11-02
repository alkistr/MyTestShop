using MediatR;
using MyTestShop.Domain.Abstractions;
using MyTestShop.Infrastructure.Repositories;

namespace MyTestShop.Application.Customers.GetCustomer
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Result<GetCustomerQueryResponse>>
    {
        private readonly ICustomerRepository _customerRepository;
        public GetCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<Result<GetCustomerQueryResponse>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

            if (customer == null)
            {
                return Result<GetCustomerQueryResponse>.Failure("Customer not found");
            }

            var response = new GetCustomerQueryResponse
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                PostalCode = customer.PostalCode
            };

            return Result<GetCustomerQueryResponse>.Success("", response);
        }
    }
}
