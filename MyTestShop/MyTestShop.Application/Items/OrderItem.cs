namespace MyTestShop.Application.Items
{
    public sealed record OrderItem(
        int ProductId,
        int Quantity
    );
}
