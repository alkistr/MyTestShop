using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using MyTestShop.Application.Customers.CreateCustomer;
using MyTestShop.Application.Customers.DeleteCustmer;
using MyTestShop.Application.Customers.GetCustomer;
using MyTestShop.Application.Customers.UpdateCustomer;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.EndPoints.Customers
{
    public static class CustomersEndpoints
    {
        public static IEndpointRouteBuilder MapCustomersEndpoints(this IEndpointRouteBuilder builder)
        {
            var routeGroupBuilder = builder.MapGroup("api/customers");//.RequireAuthorization();

            routeGroupBuilder.MapGet("{id}", GetCustomer);
            routeGroupBuilder.MapPost("/", CreateCustomer);
            routeGroupBuilder.MapPut("{id}", UpdateCustomer);
            routeGroupBuilder.MapDelete("{id}", DeleteCustomer);

            return builder;
        }

        public static async Task<Results<Ok<Result<GetCustomerQueryResponse>>, NotFound<Result<GetCustomerQueryResponse>>>> GetCustomer(
            int customerId,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new GetCustomerQuery(customerId);
            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.NotFound(result);
        }

        public static async Task<Results<Ok<Result>, NotFound>> CreateCustomer(
            CreateCustomerRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new CreateCustomerCommand(
                request.FirstName,
                request.LastName,
                request.Address,
                request.PostalCode
            );

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.NotFound();
        }

        public static async Task<Results<Ok<Result>, NotFound>> UpdateCustomer(
            UpdateCustomerRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new UpdateCustomerCommand(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Address,
                request.PostalCode
            );

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.NotFound();
        }

        public static async Task<Results<Ok<Result>, NotFound>> DeleteCustomer(
            int customerId,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new DeleteCustomerCommand(
                customerId
            );

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.NotFound();
        }
    }
}
