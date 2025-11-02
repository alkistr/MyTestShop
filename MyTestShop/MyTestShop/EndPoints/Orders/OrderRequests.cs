using MyTestShop.Application.Items;

namespace MyTestShop.EndPoints.Orders
{
    internal sealed record CreateOrderRequest(
        int CustomerId,
        List<OrderItem> Items
    );

    internal sealed record UpdateOrderRequest(
        List<OrderItem> Items
    );

    internal sealed record CancelOrderRequest(
        bool CancelOrder
    );
}
