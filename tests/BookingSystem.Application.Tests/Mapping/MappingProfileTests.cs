using AutoMapper;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Mapping;
using BookingSystem.Core.Models;

namespace BookingSystem.Application.Tests.Mapping
{
public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void Should_Map_Member_To_MemberDto()
    {
        // Arrange
        var member = new Member
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateJoined = DateTime.Parse("2024-01-02T12:10:11"),
            Bookings = new List<Booking>
            {
                new Booking { IsCancelled = false },
                new Booking { IsCancelled = true },
                new Booking { IsCancelled = false }
            }
        };

        // Act
        var dto = _mapper.Map<MemberDto>(member);

        // Assert
        Assert.Equal(member.Id, dto.Id);
        Assert.Equal(member.FirstName, dto.FirstName);
        Assert.Equal(member.LastName, dto.LastName);
        Assert.Equal(member.Email, dto.Email);
        Assert.Equal(member.DateJoined, dto.DateJoined);
        Assert.Equal(2, dto.ActiveBookingsCount); // Only non-cancelled bookings
    }

    [Fact]
    public void Should_Map_InventoryItem_To_InventoryItemDto()
    {
        // Arrange
        var inventoryItem = new InventoryItem
        {
            Id = 1,
            Name = "Test Item",
            Description = "Test Description",
            RemainingCount = 10,
            Bookings = new List<Booking>
            {
                new Booking { IsCancelled = false },
                new Booking { IsCancelled = false },
                new Booking { IsCancelled = true }
            }
        };

        // Act
        var dto = _mapper.Map<InventoryItemDto>(inventoryItem);

        // Assert
        Assert.Equal(inventoryItem.Id, dto.Id);
        Assert.Equal(inventoryItem.Name, dto.Name);
        Assert.Equal(inventoryItem.Description, dto.Description);
        Assert.Equal(inventoryItem.RemainingCount, dto.RemainingCount);
    }

    [Fact]
    public void Should_Map_Booking_To_BookingDto()
    {
        // Arrange
        var bookingDate = DateTime.UtcNow;
        var booking = new Booking
        {
            Id = 1,
            ReferenceNumber = "BK-001",
            BookingDate = bookingDate,
            IsCancelled = false,
            Member = new Member { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
            InventoryItem = new InventoryItem { Id = 1, Name = "Test Item", RemainingCount = 5 }
        };

        // Act
        var dto = _mapper.Map<BookingDto>(booking);

        // Assert
        Assert.Equal(booking.Id, dto.Id);
        Assert.Equal(booking.ReferenceNumber, dto.ReferenceNumber);
        Assert.Equal(booking.BookingDate, dto.BookingDate);
        Assert.Equal(booking.IsCancelled, dto.IsCancelled);
        Assert.NotNull(dto.Member);
        Assert.NotNull(dto.InventoryItem);
    }
}
}
