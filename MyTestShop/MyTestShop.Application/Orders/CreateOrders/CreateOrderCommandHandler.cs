using MediatR;
using MyTestShop.Domain.Abstractions;
using System.Linq;
using MyTestShop.Domain.Orders;
using MyTestShop.Infrastructure.Repositories;
using MyTestShop.Domain.Items;
using MyTestShop.Domain.Products;

namespace MyTestShop.Application.Orders.CreateOrders
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, ICustomerRepository customerRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
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

            // Ensure referenced products exist
            var missingProductIds = request.Items
                .Select(i => i.ProductId)
                .Where(id => !productDictionary.ContainsKey(id))
                .Distinct()
                .ToList();

            if (missingProductIds.Any())
            {
                return Result.Failure($"Products not found: {string.Join(',', missingProductIds)}");
            }

            var order = Order.CreateNew(
                request.Items.Select(item => Item.CreateNew(
                    productDictionary[item.ProductId],
                    item.Quantity)).ToList()
            );

            if (customer.Orders == null)
            {
                customer.Orders = new List<Order>();
            }

            customer.Orders.Add(order);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0 ? 
                Result.Success($"Successfully created order {order.Id}") :
                Result.Failure("Failed to create order.");
        }
    }
}
