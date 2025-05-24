using BookingSystem.Core.Models;

namespace BookingSystem.Core.Tests.Models;

public class MemberTests
{
    [Fact]
    public void Member_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var member = new Member
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            BookingCount = 1
        };

        // Assert
        Assert.Equal(1, member.Id);
        Assert.Equal("John", member.FirstName);
        Assert.Equal("Doe", member.LastName);
        Assert.Equal("john.doe@example.com", member.Email);
        Assert.Equal(1, member.BookingCount);
    }

    [Fact]
    public void ActiveBookings_ShouldReturnOnlyNonCancelledBookings()
    {
        // Arrange
        var member = new Member { Id = 1 };
        var activeBooking = new Booking { Id = 1, MemberId = 1, IsCancelled = false };
        var cancelledBooking = new Booking { Id = 2, MemberId = 1, IsCancelled = true };

        member.Bookings.Add(activeBooking);
        member.Bookings.Add(cancelledBooking);

        // Act
        var activeBookings = member.ActiveBookings;

        // Assert
        Assert.Single(activeBookings);
        Assert.Contains(activeBooking, activeBookings);
        Assert.DoesNotContain(cancelledBooking, activeBookings);
    }
}
