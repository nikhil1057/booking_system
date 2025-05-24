using BookingSystem.Application.Commands;
using BookingSystem.Application.Handlers;
using BookingSystem.Core.Interfaces;
using BookingSystem.Core.Models;
using Moq;
using Xunit;

namespace BookingSystem.Core.Tests.Handlers
{
    public class CancelBookingCommandHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepoMock;
        private readonly Mock<IMemberRepository> _memberRepoMock;
        private readonly Mock<IInventoryRepository> _inventoryRepoMock;
        private readonly CancelBookingCommandHandler _handler;

        public CancelBookingCommandHandlerTests()
        {
            _bookingRepoMock = new Mock<IBookingRepository>();
            _memberRepoMock = new Mock<IMemberRepository>();
            _inventoryRepoMock = new Mock<IInventoryRepository>();

            _handler = new CancelBookingCommandHandler(
                _memberRepoMock.Object,
                _bookingRepoMock.Object,
                _inventoryRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenBookingNotFound()
        {
            _bookingRepoMock.Setup(r => r.GetByReferenceNumberAsync("invalid-ref")).ReturnsAsync((Booking)null);

            var command = new CancelBookingCommand("invalid-ref");
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Booking with reference number invalid-ref not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenMemberNotFound()
        {
            var booking = new Booking { ReferenceNumber = "ref123", MemberId = 1 };
            _bookingRepoMock.Setup(r => r.GetByReferenceNumberAsync("ref123")).ReturnsAsync(booking);
            _memberRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Member)null);

            var result = await _handler.Handle(new CancelBookingCommand("ref123"), CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Member with booking not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenInventoryNotFound()
        {
            var booking = new Booking { ReferenceNumber = "ref123", MemberId = 1, InventoryItemId = 2 };
            var member = new Member { Id = 1 };

            _bookingRepoMock.Setup(r => r.GetByReferenceNumberAsync("ref123")).ReturnsAsync(booking);
            _memberRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(member);
            _inventoryRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((InventoryItem)null);

            var result = await _handler.Handle(new CancelBookingCommand("ref123"), CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Inventory item with booking not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenBookingAlreadyCancelled()
        {
            var booking = new Booking { ReferenceNumber = "ref123", MemberId = 1, InventoryItemId = 2, IsCancelled = true };
            var member = new Member { Id = 1 };
            var inventory = new InventoryItem { Id = 2 };

            _bookingRepoMock.Setup(r => r.GetByReferenceNumberAsync("ref123")).ReturnsAsync(booking);
            _memberRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(member);
            _inventoryRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(inventory);

            var result = await _handler.Handle(new CancelBookingCommand("ref123"), CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Booking with reference number ref123 is already cancelled.", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ShouldCancelBooking_WhenValid()
        {
            var booking = new Booking
            {
                ReferenceNumber = "ref123",
                MemberId = 1,
                InventoryItemId = 2,
                IsCancelled = false
            };

            var member = new Member { Id = 1, BookingCount = 2 };
            var inventory = new InventoryItem { Id = 2, RemainingCount = 5 };

            _bookingRepoMock.Setup(r => r.GetByReferenceNumberAsync("ref123")).ReturnsAsync(booking);
            _memberRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(member);
            _inventoryRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(inventory);

            var result = await _handler.Handle(new CancelBookingCommand("ref123"), CancellationToken.None);

            Assert.True(result.Success);
            Assert.True(booking.IsCancelled);
            Assert.NotNull(booking.CancellationDate);
            Assert.Equal(1, member.BookingCount);
            Assert.Equal(6, inventory.RemainingCount);

            _bookingRepoMock.Verify(r => r.UpdateAsync(booking), Times.Once);
            _memberRepoMock.Verify(r => r.UpdateAsync(member), Times.Once);
            _inventoryRepoMock.Verify(r => r.UpdateAsync(inventory), Times.Once);
        }
    }
}
