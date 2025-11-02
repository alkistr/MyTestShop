namespace MyTestShop.EndPoints.Customers
{
    public sealed record CreateCustomerRequest(
        string FirstName,
        string LastName,
        string Address,
        string PostalCode
    );

    public sealed record UpdateCustomerRequest(
        int Id,
        string FirstName,
        string LastName,
        string Address,
        string PostalCode
    );
}
