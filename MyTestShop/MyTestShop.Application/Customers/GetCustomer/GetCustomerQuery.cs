using MediatR;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.Application.Customers.GetCustomer
{
    public sealed record GetCustomerQuery(int Id) : IRequest<Result<GetCustomerQueryResponse>>;    
}
