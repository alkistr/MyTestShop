using MyTestShop.Application.Items;

namespace MyTestShop.Application.Orders.GetOrders
{
    public sealed record GetOrderQueryResponse(
        int OrderId,
        DateTime OrderDate,
        List<OrderItem> Items,
        decimal TotalPrice,
        bool Cancelled
    );
}
