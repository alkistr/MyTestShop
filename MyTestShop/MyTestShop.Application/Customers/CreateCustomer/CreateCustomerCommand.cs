using MediatR;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.Application.Customers.CreateCustomer
{
    public sealed record CreateCustomerCommand(
        int Id,
        string FirstName,
        string LastName,
        string Address,
        string PostalCode
    ) : IRequest<Result>;    
}
