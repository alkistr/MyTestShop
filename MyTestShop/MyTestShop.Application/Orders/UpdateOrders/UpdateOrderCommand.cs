using MediatR;
using MyTestShop.Application.Items;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.Application.Orders.UpdateOrders
{
    public sealed record UpdateOrderCommand(
        int OrderId,
        List<OrderItem> Items
        ) : IRequest<Result>;
}
