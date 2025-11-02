using MyTestShop.Domain.Orders;

namespace MyTestShop.Domain.Customers
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public List<Order> Orders { get; set; }

        public static Customer CreateNew(string firstName, string lastName, string address, string postalCode)
        {
            return new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                PostalCode = postalCode,
                Orders = new List<Order>()
            };
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, Address: {Address}, PostalCode: {PostalCode}";
        }
    }

    //We could use a value object for Address if needed in the future
    //public class Address
    //{
    //    public string Street { get; set; }
    //    public string City { get; set; }
    //    public string State { get; set; }
    //    public string PostalCode { get; set; }
    //    public string Country { get; set; }
    //}
}
