using MediatR;
using MyTestShop.Application.Items;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.Application.Orders.CreateOrders
{
    public sealed record CreateOrderCommand(
        int CustomerId,
        List<OrderItem> Items
    ): IRequest<Result>;
}
