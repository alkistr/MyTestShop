using MediatR;
using MyTestShop.Domain.Abstractions;
using MyTestShop.Infrastructure.Repositories;

namespace MyTestShop.Application.Customers.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer is null)
            {
                return Result.Failure("Customer not found.");
            }
            _customerRepository.Delete(customer);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0 ? Result.Success("Successfully deleted customer.") : Result.Failure("Failed to delete customer.");
        }
    }
}
