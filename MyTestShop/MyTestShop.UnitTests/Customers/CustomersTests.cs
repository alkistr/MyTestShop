using Moq;
using MyTestShop.Application.Customers.CreateCustomer;
using MyTestShop.Application.Customers.DeleteCustomer;
using MyTestShop.Application.Customers.GetCustomer;
using MyTestShop.Application.Customers.UpdateCustomer;
using MyTestShop.Domain.Abstractions;
using MyTestShop.Domain.Customers;
using MyTestShop.Infrastructure.Repositories;

namespace MyTestShop.UnitTests.Customers
{
    [TestFixture]
    public class CustomersTests
    {
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void SetUp()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        #region CreateCustomer Tests

        [Test]
        public async Task CreateCustomer_ValidRequest_CreatesCustomerSuccessfully()
        {
            // Arrange
            var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new CreateCustomerCommand("John", "Doe", "123 Main St", "12345");

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Message, Does.Contain("John Doe"));
            _customerRepositoryMock.Verify(x => x.Add(It.IsAny<Customer>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task CreateCustomer_SaveChangesFails_ReturnsFailure()
        {
            // Arrange
            var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new CreateCustomerCommand("John", "Doe", "123 Main St", "12345");

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Failed to create customer."));
        }

        #endregion

        #region GetCustomer Tests

        [Test]
        public async Task GetCustomer_CustomerExists_ReturnsCustomerSuccessfully()
        {
            // Arrange
            var handler = new GetCustomerQueryHandler(_customerRepositoryMock.Object);
            var query = new GetCustomerQuery(1);
            var customer = Customer.CreateNew("Jane", "Smith", "456 Oak Ave", "67890");
            customer.Id = 1;

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public async Task GetCustomer_CustomerNotFound_ReturnsFailure()
        {
            // Arrange
            var handler = new GetCustomerQueryHandler(_customerRepositoryMock.Object);
            var query = new GetCustomerQuery(999);

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Customer not found"));
        }

        #endregion

        #region UpdateCustomer Tests

        [Test]
        public async Task UpdateCustomer_CustomerExists_UpdatesSuccessfully()
        {
            // Arrange
            var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _unitOfWorkMock.Object);
            var customer = Customer.CreateNew("John", "Doe", "123 Main St", "12345");
            customer.Id = 1;
            var command = new UpdateCustomerCommand(1, "Jane", "Smith", "456 Oak Ave", "67890");

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(customer.FirstName, Is.EqualTo("Jane"));
            Assert.That(customer.LastName, Is.EqualTo("Smith"));
            Assert.That(customer.Address, Is.EqualTo("456 Oak Ave"));
            Assert.That(customer.PostalCode, Is.EqualTo("67890"));
            Assert.That(result.Message, Does.Contain("Jane Smith"));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateCustomer_CustomerNotFound_ReturnsFailure()
        {
            // Arrange
            var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new UpdateCustomerCommand(999, "Jane", "Smith", "456 Oak Ave", "67890");

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Customer not found."));
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task UpdateCustomer_SaveChangesFails_ReturnsFailure()
        {
            // Arrange
            var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _unitOfWorkMock.Object);
            var customer = Customer.CreateNew("John", "Doe", "123 Main St", "12345");
            customer.Id = 1;
            var command = new UpdateCustomerCommand(1, "Jane", "Smith", "456 Oak Ave", "67890");

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Failed to update customer."));
        }

        #endregion

        #region DeleteCustomer Tests

        [Test]
        public async Task DeleteCustomer_CustomerExists_DeletesSuccessfully()
        {
            // Arrange
            var handler = new DeleteCustomerCommandHandler(_customerRepositoryMock.Object, _unitOfWorkMock.Object);
            var customer = Customer.CreateNew("John", "Doe", "123 Main St", "12345");
            customer.Id = 1;
            var command = new DeleteCustomerCommand(1);

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Message, Is.EqualTo("Successfully deleted customer."));
            _customerRepositoryMock.Verify(x => x.Delete(customer), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteCustomer_CustomerNotFound_ReturnsFailure()
        {
            // Arrange
            var handler = new DeleteCustomerCommandHandler(_customerRepositoryMock.Object, _unitOfWorkMock.Object);
            var command = new DeleteCustomerCommand(999);

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Customer not found."));
            _customerRepositoryMock.Verify(x => x.Delete(It.IsAny<Customer>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DeleteCustomer_SaveChangesFails_ReturnsFailure()
        {
            // Arrange
            var handler = new DeleteCustomerCommandHandler(_customerRepositoryMock.Object, _unitOfWorkMock.Object);
            var customer = Customer.CreateNew("John", "Doe", "123 Main St", "12345");
            customer.Id = 1;
            var command = new DeleteCustomerCommand(1);

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Failed to delete customer."));
        }

        #endregion
    }
}
