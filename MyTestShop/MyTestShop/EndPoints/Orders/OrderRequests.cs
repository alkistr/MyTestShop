using MyTestShop.Application.Items;

namespace MyTestShop.EndPoints.Orders
{
    internal sealed record CreateOrderRequest(
        int CustomerId,
        List<OrderItem> Items
    );

    internal sealed record UpdateOrderRequest(
        int OrderId,
        List<OrderItem> Items
    );
}
