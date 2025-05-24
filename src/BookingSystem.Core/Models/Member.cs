using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Core.Models;

public class Member
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateJoined { get; set; }

    public int BookingCount { get; set; }
    public List<Booking> Bookings { get; set; } = new List<Booking>();

    // Navigation property
    [NotMapped]
    public virtual ICollection<Booking> ActiveBookings => Bookings.Where(b => !b.IsCancelled).ToList();
}
