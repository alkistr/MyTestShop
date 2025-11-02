using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using MyTestShop.Application.Orders.CancelOrders;
using MyTestShop.Application.Orders.CreateOrders;
using MyTestShop.Application.Orders.GetOrders;
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
            routeGroupBuilder.MapPut("{orderId}", UpdateOrder);
            routeGroupBuilder.MapPut("{orderId}/cancel", CancelOrder);

            return builder;
        }

        internal static async Task<Results<Ok<Result<GetOrderQueryResponse>>, NotFound<Result<GetOrderQueryResponse>>>> GetOrder(
            int orderId,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new GetOrderQuery(orderId);
            var result = await sender.Send(query, cancellationToken);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.NotFound(result);
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
            int orderId,
            UpdateOrderRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new UpdateOrderCommand(
                orderId,
                request.Items
            );
            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest();
        }

        internal static async Task<Results<Ok<Result>, BadRequest>> CancelOrder(
            int orderId,
            CancelOrderRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new CancelOrderCommand(
                orderId,
                request.CancelOrder
            );

            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest();
        }
    }
}
