using MediatR;
using MyTestShop.Domain.Abstractions;
using MyTestShop.Domain.Orders;
using MyTestShop.Infrastructure.Repositories;
using MyTestShop.Domain.Items;

namespace MyTestShop.Application.Orders.CreateOrders
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

            if (customer == null)
            {
                return Result.Failure("Customer not found");
            }

            var productDictionary = await _productRepository.LoadProductsAsync();

            var order = Order.CreateNew(
                request.Items.Select(item => Item.CreateNew(
                    productDictionary.GetValueOrDefault(item.ProductId), // if the product is not found, null will be passed, this should be handled
                    item.Quantity)).ToList()
            );

            customer.Orders.Add(order);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0 ? 
                Result.Success($"Successfully created order {order.Id}") :
                Result.Failure("Failed to create order.");
        }
    }
}
