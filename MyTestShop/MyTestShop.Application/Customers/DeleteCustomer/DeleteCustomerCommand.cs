using MediatR;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.Application.Customers.DeleteCustomer
{
    public sealed record DeleteCustomerCommand(int CustomerId) : IRequest<Result>;
}
