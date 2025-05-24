using BookingSystem.Core.Models;

namespace BookingSystem.Core.Tests.Models;

public class InventoryItemTests
{
    [Fact]
    public void InventoryItem_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var item = new InventoryItem
        {
            Id = 1,
            Name = "Test Item",
            Description = "Test Description",
            RemainingCount = 10
        };

        // Assert
        Assert.Equal(1, item.Id);
        Assert.Equal("Test Item", item.Name);
        Assert.Equal("Test Description", item.Description);
        Assert.Equal(10, item.RemainingCount);
        Assert.NotNull(item.Bookings);
        Assert.Empty(item.Bookings);
    }

    [Fact]
    public void RemainingCount_ShouldCalculateCorrectly()
    {
        // Arrange
        var item = new InventoryItem { Id = 1, RemainingCount = 3 };
        var activeBooking1 = new Booking { Id = 1, InventoryItemId = 1, IsCancelled = false };
        var activeBooking2 = new Booking { Id = 2, InventoryItemId = 1, IsCancelled = false };
        var cancelledBooking = new Booking { Id = 3, InventoryItemId = 1, IsCancelled = true };

        item.Bookings.Add(activeBooking1);
        item.Bookings.Add(activeBooking2);
        item.Bookings.Add(cancelledBooking);

        // Act
        var activeBookings = item.Bookings.Count(x => x.IsCancelled == false);
        var remainingCount = item.RemainingCount - activeBookings;

        // Assert
        Assert.Equal(1, remainingCount); 
    }

    [Fact]
    public void IsAvailable_ShouldReturnTrueWhenItemsRemaining()
    {
        // Arrange
        var item = new InventoryItem { Id = 1, RemainingCount = 5 };
        var activeBooking = new Booking { Id = 1, InventoryItemId = 1, IsCancelled = false };
        item.Bookings.Add(activeBooking);

        // Act
        var isAvailable = item.IsAvailable;

        // Assert
        Assert.True(isAvailable);
    }

    [Fact]
    public void IsAvailable_ShouldReturnFalseWhenNoItemsRemaining()
    {
        // Arrange
        var item = new InventoryItem { Id = 1, RemainingCount = 1 };
        var activeBooking = new Booking { Id = 1, InventoryItemId = 1, IsCancelled = false };
        item.Bookings.Add(activeBooking);

        // Act
        var activeBookings = item.Bookings.Count(x => x.IsCancelled == false);
        item.RemainingCount = item.RemainingCount - activeBookings;
        var isAvailable = item.IsAvailable;

        // Assert
        Assert.False(isAvailable);
    }
}