namespace MyTestShop.EndPoints.Orders
{
    public static class OrdersEndpoints
    {
        public static IEndpointRouteBuilder MapOrdersEndpoints(this IEndpointRouteBuilder builder)
        {
            var routeGroupBuilder = builder.MapGroup("api/orders").RequireAuthorization();

            //routeGroupBuilder.MapGet("{id}", GetOrder);

            return builder;
        }
    }
}
