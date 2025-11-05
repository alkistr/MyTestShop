using MediatR;
using MyTestShop.Domain.Abstractions;
using MyTestShop.Infrastructure.Repositories;

namespace MyTestShop.Application.Orders.CancelOrders
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Result>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                return Result.Failure($"Order with ID {request.OrderId} not found.");
            }

            order.Cancelled = request.Cancel;
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0 ?
                Result.Success("Order cancelled successfully.") : 
                Result.Failure("Failed to cancel order.");
        }
    }
}
