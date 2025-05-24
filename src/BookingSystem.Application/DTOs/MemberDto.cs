namespace BookingSystem.Application.DTOs;

public class MemberDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateJoined { get; set; }

    public int BookingCount { get; set; }
    public int ActiveBookingsCount { get; set; }
}
