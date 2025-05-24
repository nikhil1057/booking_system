using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Core.Models;

public class InventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int RemainingCount { get; set; }

    public DateTime ExpirationDate { get; set; }
    public List<Booking> Bookings { get; set; } = new List<Booking>();

    // Navigation property

    public bool IsAvailable => RemainingCount > 0;
}
