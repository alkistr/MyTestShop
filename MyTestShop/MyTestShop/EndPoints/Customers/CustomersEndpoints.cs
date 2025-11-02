using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using MyTestShop.Application.Customers.CreateCustomer;
using MyTestShop.Application.Customers.DeleteCustomer;
using MyTestShop.Application.Customers.GetCustomer;
using MyTestShop.Application.Customers.UpdateCustomer;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.EndPoints.Customers
{
    internal static class CustomersEndpoints
    {
        internal static IEndpointRouteBuilder MapCustomersEndpoints(this IEndpointRouteBuilder builder)
        {
            var routeGroupBuilder = builder.MapGroup("api/customers");//.RequireAuthorization();

            routeGroupBuilder.MapGet("{id}", GetCustomer);
            routeGroupBuilder.MapPost("/", CreateCustomer);
            routeGroupBuilder.MapPut("/", UpdateCustomer);
            routeGroupBuilder.MapDelete("{id}", DeleteCustomer);

            return builder;
        }

        internal static async Task<Results<Ok<Result<GetCustomerQueryResponse>>, NotFound<Result<GetCustomerQueryResponse>>>> GetCustomer(
            int customerId,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new GetCustomerQuery(customerId);
            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.NotFound(result);
        }

        internal static async Task<Results<Ok<Result>, BadRequest>> CreateCustomer(
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

            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest();
        }

        internal static async Task<Results<Ok<Result>, NotFound>> UpdateCustomer(
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

        internal static async Task<Results<Ok<Result>, NotFound>> DeleteCustomer(
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
