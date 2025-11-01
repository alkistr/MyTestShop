using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.HttpResults;
using MyTestShop.Application.Customers.CreateCustomer;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.EndPoints.Customers
{
    public static class CustomersEndpoints
    {
        public static IEndpointRouteBuilder MapCustomersEndpoints(this IEndpointRouteBuilder builder)
        {
            var routeGroupBuilder = builder.MapGroup("api/customers").RequireAuthorization();

            routeGroupBuilder.MapGet("{id}", GetCustomer);

            return builder;
        }

        public static async Task<Results<Ok<Result>, NotFound>> GetCustomer(int id,
        ISender sender,
        CancellationToken cancellationToken)
        {
            // Simulate fetching customer data
            var customer = new
            {
                Id = id,
                Name = "John Doe",
                Email = "john.doe@example.com"
            };

            return Results.Ok(customer);
        }

        public static async Task<Results<Ok<Result>, NotFound>> CreateCustomer(
            CreateCustomerRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new CreateCustomerCommand(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Address,
                request.PostalCode
            );

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.NotFound();
        }

        public static async Task<Results<Ok<Result>, NotFound>> UpdateCustomer(int id)
        {
            // Simulate fetching customer data
            var customer = new
            {
                Id = id,
                Name = "John Doe",
                Email = "john.doe@example.com"
            };

            return TypedResults.Ok();
        }

        public static async Task<Results<Ok<Result>, NotFound>> DeleteCustomer(int id)
        {
            // Simulate fetching customer data
            var customer = new
            {
                Id = id,
                Name = "John Doe",
                Email = "john.doe@example.com"
            };

            return Results.Ok(customer);
        }
    }
}
