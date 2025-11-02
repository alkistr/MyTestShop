using MediatR;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.Application.Customers.DeleteCustmer
{
    public sealed record DeleteCustomerCommand(int CustomerId) : IRequest<Result>;
}
