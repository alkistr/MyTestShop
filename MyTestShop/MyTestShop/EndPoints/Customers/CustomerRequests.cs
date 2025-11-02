namespace MyTestShop.EndPoints.Customers
{
    internal sealed record CreateCustomerRequest(
        string FirstName,
        string LastName,
        string Address,
        string PostalCode
    );

    internal sealed record UpdateCustomerRequest(
        int Id,
        string FirstName,
        string LastName,
        string Address,
        string PostalCode
    );
}
