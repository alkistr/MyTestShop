using MediatR;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.Application.Orders.CancelOrders
{
    public sealed record CancelOrderCommand(
        int OrderId,
        bool Cancel) : IRequest<Result>;
}
