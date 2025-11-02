using MediatR;
using MyTestShop.Domain.Abstractions;
using MyTestShop.Domain.Items;
using MyTestShop.Domain.Orders;
using MyTestShop.Infrastructure.Repositories;

namespace MyTestShop.Application.Orders.UpdateOrders
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);

            if (order == null)
            {
                return Result.Failure("Order not found");
            }

            var productDictionary = await _productRepository.LoadProductsAsync();

            order = Order.Update(order,
                                request.Items.Select(item => Item.CreateNew
                                (
                                    productDictionary.GetValueOrDefault(item.ProductId),// if the product is not found, null will be passed, this should be handled
                                    item.Quantity 
                                )).ToList()
                );

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0 ?
                Result.Success($"Successfully updated order {order.Id}") :
                Result.Failure("Failed to update order.");
        }
    }
}
