using MyTestShop.Domain.Products;

namespace MyTestShop.Domain.Items
{
    public class Item
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public static Item CreateNew(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
            
            return new Item
            {
                Product = product,
                Quantity = quantity
            };
        }
    }
}
