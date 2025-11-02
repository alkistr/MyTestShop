using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using MyTestShop.Application.Orders.CreateOrders;
using MyTestShop.Application.Orders.UpdateOrders;
using MyTestShop.Domain.Abstractions;

namespace MyTestShop.EndPoints.Orders
{
    internal static class OrdersEndpoints
    {
        internal static IEndpointRouteBuilder MapOrdersEndpoints(this IEndpointRouteBuilder builder)
        {
            var routeGroupBuilder = builder.MapGroup("api/orders");//.RequireAuthorization();

            routeGroupBuilder.MapGet("{id}", GetOrder);
            routeGroupBuilder.MapPost("", CreateOrder);
            routeGroupBuilder.MapPut("", UpdateOrder);

            return builder;
        }

        internal static async Task<Results<Ok<Result>, NotFound>> GetOrder(
            int orderId,
            ISender sender,
            CancellationToken cancellationToken)
        {
            //var query = new GetOrderQuery(orderId);
            //var result = await sender.Send(query, cancellationToken);
            //return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.NotFound(result);
            return TypedResults.NotFound();
        }

        internal static async Task<Results<Ok<Result>, BadRequest>> CreateOrder(
            CreateOrderRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new CreateOrderCommand(
                request.CustomerId,
                request.Items
            );
            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest();
        }

        internal static async Task<Results<Ok<Result>, BadRequest>> UpdateOrder(
            UpdateOrderRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new UpdateOrderCommand(
                request.OrderId,
                request.Items
            );
            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest();
        }
    }
}
