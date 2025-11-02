using MediatR;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.Application.Orders.GetOrders
{
    public sealed record GetOrderQuery(int OrderId) : IRequest<Result<GetOrderQueryResponse>>;
}
