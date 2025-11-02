using MediatR;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.Application.Customers.UpdateCustomer
{
    public sealed record UpdateCustomerCommand(
        int Id,
        string FirstName,
        string LastName,
        string Address,
        string PostalCode
    ) : IRequest<Result>;    
}
