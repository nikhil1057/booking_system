using BookingSystem.Application.Commands;
using BookingSystem.Application.Handlers;
using BookingSystem.Core.Interfaces;
using BookingSystem.Core.Models;
using Moq;
using Xunit;

namespace BookingSystem.Core.Tests.Handlers
{
    public class CreateBookingCommandHandlerTests
    {
        private readonly Mock<IMemberRepository> _memberRepositoryMock;
        private readonly Mock<IInventoryRepository> _inventoryRepositoryMock;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly CreateBookingCommandHandler _handler;

        public CreateBookingCommandHandlerTests()
        {
            _memberRepositoryMock = new Mock<IMemberRepository>();
            _inventoryRepositoryMock = new Mock<IInventoryRepository>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();

            _handler = new CreateBookingCommandHandler(
                _memberRepositoryMock.Object,
                _inventoryRepositoryMock.Object,
                _bookingRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_MemberNotFound_ShouldReturnFailure()
        {
            // Arrange
            var command = new CreateBookingCommand("nonexistent@example.com", "Test Item");
            _memberRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Member?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_MemberHasMaxBookings_ShouldReturnFailure()
        {
            // Arrange
            var member = new Member
            {
                Id = 1,
                Email = "test@example.com",
                Bookings = new List<Booking>
            {
                new Booking { IsCancelled = false },
                new Booking { IsCancelled = false }
            }
            };

            var command = new CreateBookingCommand("test@example.com", "Test Item");

            _memberRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(member);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("maximum number of bookings", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_InventoryItemNotFound_ShouldReturnFailure()
        {
            // Arrange
            var member = new Member { Id = 1, Email = "test@example.com" };
            var command = new CreateBookingCommand("test@example.com", "Nonexistent Item");

            _memberRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(member);
            _inventoryRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((InventoryItem?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("not found", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_InventoryItemNotAvailable_ShouldReturnFailure()
        {
            // Arrange
            var member = new Member { Id = 1, Email = "test@example.com" };
            var inventoryItem = new InventoryItem
            {
                Id = 1,
                Name = "Test Item",
                RemainingCount = 0,
                Bookings = new List<Booking> { new Booking { IsCancelled = false } }
            };

            var command = new CreateBookingCommand("test@example.com", "Test Item");

            _memberRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(member);
            _inventoryRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(inventoryItem);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("not available", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnSuccessWithBookingReference()
        {
            // Arrange
            var member = new Member { Id = 1, Email = "test@example.com" };
            var inventoryItem = new InventoryItem
            {
                Id = 1,
                Name = "Test Item",
                RemainingCount = 5,
                Bookings = new List<Booking>()
            };

            var command = new CreateBookingCommand("test@example.com", "Test Item");

            _memberRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(member);
            _inventoryRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(inventoryItem);
            _bookingRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Booking>()))
                .ReturnsAsync((Booking b) => b);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.BookingReference);
            Assert.StartsWith("BK-", result.BookingReference);
            _bookingRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Booking>()), Times.Once);
        }
    }
}
