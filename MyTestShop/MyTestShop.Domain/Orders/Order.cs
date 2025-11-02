using MyTestShop.Domain.Items;

namespace MyTestShop.Domain.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Item> Items { get; set; }
        public bool Cancelled { get; set; }

        private decimal? _overrideTotalPrice;
        public decimal TotalPrice
        {
            get
            {
                decimal computed = Items?.Sum(i => (i?.Product?.Price ?? 0m) * i.Quantity) ?? 0m;
                return _overrideTotalPrice ?? computed;
            }
            set
            {
                if (value < 0m)
                    throw new ArgumentOutOfRangeException(nameof(value), "Total price cannot be negative.");

                _overrideTotalPrice = value;
            }
        }

        public static Order CreateNew(List<Item> items)
        {
            return new Order
            {
                Items = items,
                OrderDate = DateTime.UtcNow,
                Cancelled = false
            };
        }

        public static Order Update(Order existingOrder, List<Item> newItems)
        {
            existingOrder.Items = newItems;
            return existingOrder;
        }
    }
}
