using BookingSystem.Core.Models;

namespace BookingSystem.Core.Tests.Models;

public class BookingTests
{
    [Fact]
    public void Booking_ShouldInitializeCorrectly()
    {
        // Arrange
        var bookingDate = DateTime.UtcNow;

        // Act
        var booking = new Booking
        {
            Id = 1,
            ReferenceNumber = "BK-001",
            BookingDate = bookingDate,
            IsCancelled = false,
            MemberId = 1,
            InventoryItemId = 1
        };

        // Assert
        Assert.Equal(1, booking.Id);
        Assert.Equal("BK-001", booking.ReferenceNumber);
        Assert.Equal(bookingDate, booking.BookingDate);
        Assert.False(booking.IsCancelled);
        Assert.Null(booking.CancellationDate);
        Assert.Equal(1, booking.MemberId);
        Assert.Equal(1, booking.InventoryItemId);
    }

    [Fact]
    public void Booking_WhenCancelled_ShouldHaveCancellationDate()
    {
        // Arrange
        var booking = new Booking { Id = 1 };
        var cancellationDate = DateTime.UtcNow;

        // Act
        booking.IsCancelled = true;
        booking.CancellationDate = cancellationDate;

        // Assert
        Assert.True(booking.IsCancelled);
        Assert.Equal(cancellationDate, booking.CancellationDate);
    }
}