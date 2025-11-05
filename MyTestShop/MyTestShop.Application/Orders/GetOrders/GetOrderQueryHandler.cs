using MediatR;
using MyTestShop.Application.Items;
using MyTestShop.Application.Orders.GetOrders;
using MyTestShop.Domain.Abstractions;
using MyTestShop.Infrastructure.Repositories;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Result<GetOrderQueryResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<GetOrderQueryResponse>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);

        if (order == null)
        {
            return Result<GetOrderQueryResponse>.Failure("Order not found.");
        }

        var response = new GetOrderQueryResponse
        (
            order.Id,
            order.OrderDate,
            order.Items.Select(i => new OrderItem(i.Product.Id, i.Quantity)).ToList(),
            order.TotalPrice,//Items.Sum(i => i.Product.Price * i.Quantity),
            order.Cancelled
        );

        return Result<GetOrderQueryResponse>.Success("", response);
    }
}
