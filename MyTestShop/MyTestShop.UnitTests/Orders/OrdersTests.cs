using Moq;
using MyTestShop.Application.Items;
using MyTestShop.Application.Orders.CancelOrders;
using MyTestShop.Application.Orders.CreateOrders;
using MyTestShop.Application.Orders.GetOrders;
using MyTestShop.Application.Orders.UpdateOrders;
using MyTestShop.Domain.Abstractions;
using MyTestShop.Domain.Customers;
using MyTestShop.Domain.Items;
using MyTestShop.Domain.Orders;
using MyTestShop.Domain.Products;
using MyTestShop.Infrastructure.Repositories;

namespace MyTestShop.UnitTests.Orders
{
    [TestFixture]
    public class OrdersTests
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void SetUp()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        #region CreateOrder Tests

        [Test]
        public async Task CreateOrder_ValidRequest_CreatesOrderSuccessfully()
        {
            // Arrange
            var handler = new CreateOrderCommandHandler(
                _orderRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object);

            var customer = Customer.CreateNew("John", "Doe", "123 Main St", "12345");
            var product = new Product { Id = 1, Name = "Test Product", Price = 10.00m };
            var productDictionary = new Dictionary<int, Product> { { 1, product } };

            var command = new CreateOrderCommand(1, new List<OrderItem>
            {
                new OrderItem(1, 2)
            });

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);
            _productRepositoryMock.Setup(x => x.LoadProductsAsync())
                .ReturnsAsync(productDictionary);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Message, Does.Contain("Successfully created order"));
            Assert.That(customer.Orders, Has.Count.EqualTo(1));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task CreateOrder_CustomerNotFound_ReturnsFailure()
        {
            // Arrange
            var handler = new CreateOrderCommandHandler(
                _orderRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object);

            var command = new CreateOrderCommand(1, new List<OrderItem>());
            _customerRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Customer not found"));
        }

        [Test]
        public async Task CreateOrder_ProductNotFound_ReturnsFailure()
        {
            // Arrange
            var handler = new CreateOrderCommandHandler(
                _orderRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object);

            var customer = Customer.CreateNew("John", "Doe", "123 Main St", "12345");
            var command = new CreateOrderCommand(1, new List<OrderItem>
            {
                new OrderItem(999, 2)
            });

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);
            _productRepositoryMock.Setup(x => x.LoadProductsAsync())
                .ReturnsAsync(new Dictionary<int, Product>());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Does.Contain("Products not found"));
            Assert.That(result.Message, Does.Contain("999"));
        }

        [Test]
        public async Task CreateOrder_SaveChangesFails_ReturnsFailure()
        {
            // Arrange
            var handler = new CreateOrderCommandHandler(
                _orderRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object);

            var customer = Customer.CreateNew("John", "Doe", "123 Main St", "12345");
            var product = new Product { Id = 1, Name = "Test Product", Price = 10.00m };
            var productDictionary = new Dictionary<int, Product> { { 1, product } };
            
            var command = new CreateOrderCommand(1, new List<OrderItem>
            {
                new OrderItem(1, 2)
            });

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);
            _productRepositoryMock.Setup(x => x.LoadProductsAsync())
                .ReturnsAsync(productDictionary);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Failed to create order."));
        }

        #endregion

        #region GetOrder Tests

        [Test]
        public async Task GetOrder_OrderExists_ReturnsOrderSuccessfully()
        {
            // Arrange
            var handler = new GetOrderQueryHandler(_orderRepositoryMock.Object);
            var product = new Product { Id = 1, Name = "Test Product", Price = 10.00m };
            var item = Item.CreateNew(product, 2);
            var order = Order.CreateNew(new List<Item> { item });
            order.Id = 1;

            _orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var query = new GetOrderQuery(1);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public async Task GetOrder_OrderNotFound_ReturnsFailure()
        {
            // Arrange
            var handler = new GetOrderQueryHandler(_orderRepositoryMock.Object);
            _orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);

            var query = new GetOrderQuery(999);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Order not found."));
        }

        #endregion

        #region UpdateOrder Tests

        [Test]
        public async Task UpdateOrder_ValidRequest_UpdatesOrderSuccessfully()
        {
            // Arrange
            var handler = new UpdateOrderCommandHandler(
                _orderRepositoryMock.Object,
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object);

            var product1 = new Product { Id = 1, Name = "Product 1", Price = 10.00m };
            var product2 = new Product { Id = 2, Name = "Product 2", Price = 20.00m };
            var existingItem = Item.CreateNew(product1, 2);
            var existingOrder = Order.CreateNew(new List<Item> { existingItem });
            existingOrder.Id = 1;

            var productDictionary = new Dictionary<int, Product> 
            { 
                { 1, product1 },
                { 2, product2 }
            };

            var command = new UpdateOrderCommand(1, new List<OrderItem>
            {
                new OrderItem(2, 3)
            });

            _orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingOrder);
            _productRepositoryMock.Setup(x => x.LoadProductsAsync())
                .ReturnsAsync(productDictionary);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Message, Does.Contain("Successfully updated order"));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateOrder_OrderNotFound_ReturnsFailure()
        {
            // Arrange
            var handler = new UpdateOrderCommandHandler(
                _orderRepositoryMock.Object,
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object);

            var command = new UpdateOrderCommand(999, new List<OrderItem>
            {
                new OrderItem(1, 2)
            });

            _orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Order not found"));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task UpdateOrder_SaveChangesFails_ReturnsFailure()
        {
            // Arrange
            var handler = new UpdateOrderCommandHandler(
                _orderRepositoryMock.Object,
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object);

            var product = new Product { Id = 1, Name = "Product 1", Price = 10.00m };
            var existingItem = Item.CreateNew(product, 2);
            var existingOrder = Order.CreateNew(new List<Item> { existingItem });
            existingOrder.Id = 1;

            var productDictionary = new Dictionary<int, Product> { { 1, product } };
            var command = new UpdateOrderCommand(1, new List<OrderItem>
            {
                new OrderItem(1, 5)
            });

            _orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingOrder);
            _productRepositoryMock.Setup(x => x.LoadProductsAsync())
                .ReturnsAsync(productDictionary);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Failed to update order."));
        }

        #endregion

        #region CancelOrder Tests

        [Test]
        public async Task CancelOrder_ValidRequest_CancelsOrderSuccessfully()
        {
            // Arrange
            var handler = new CancelOrderCommandHandler(
                _orderRepositoryMock.Object,
                _unitOfWorkMock.Object);

            var product = new Product { Id = 1, Name = "Product 1", Price = 10.00m };
            var item = Item.CreateNew(product, 2);
            var order = Order.CreateNew(new List<Item> { item });
            order.Id = 1;
            order.Cancelled = false;

            var command = new CancelOrderCommand(1, true);

            _orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Message, Is.EqualTo("Order cancelled successfully."));
            Assert.That(order.Cancelled, Is.True);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task CancelOrder_OrderNotFound_ReturnsFailure()
        {
            // Arrange
            var handler = new CancelOrderCommandHandler(
                _orderRepositoryMock.Object,
                _unitOfWorkMock.Object);

            var command = new CancelOrderCommand(999, true);

            _orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Order with ID 999 not found."));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task CancelOrder_SaveChangesFails_ReturnsFailure()
        {
            // Arrange
            var handler = new CancelOrderCommandHandler(
                _orderRepositoryMock.Object,
                _unitOfWorkMock.Object);

            var product = new Product { Id = 1, Name = "Product 1", Price = 10.00m };
            var item = Item.CreateNew(product, 2);
            var order = Order.CreateNew(new List<Item> { item });
            order.Id = 1;

            var command = new CancelOrderCommand(1, true);

            _orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Failed to cancel order."));
        }

        [Test]
        public async Task CancelOrder_UncancelOrder_SetsToFalse()
        {
            // Arrange
            var handler = new CancelOrderCommandHandler(
                _orderRepositoryMock.Object,
                _unitOfWorkMock.Object);

            var product = new Product { Id = 1, Name = "Product 1", Price = 10.00m };
            var item = Item.CreateNew(product, 2);
            var order = Order.CreateNew(new List<Item> { item });
            order.Id = 1;
            order.Cancelled = true;

            var command = new CancelOrderCommand(1, false);

            _orderRepositoryMock.Setup(x => x.GetOrderByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(order.Cancelled, Is.False);
        }

        #endregion
    }
}
