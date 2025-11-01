using MyTestShop.Domain.Products;

namespace MyTestShop.Domain.Items
{
    public class Item
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
