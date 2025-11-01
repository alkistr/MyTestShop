namespace MyTestShop.EndPoints.Customers
{
    public sealed record CreateCustomerRequest(
        int Id,
        string FirstName,
        string LastName,
        string Address,
        string PostalCode
    );
}
