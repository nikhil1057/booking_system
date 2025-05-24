using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Core.Models;

public class Booking
{
    public int Id { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime? CancellationDate { get; set; }

    [ForeignKey(nameof(Member))]
    public int MemberId { get; set; }

    [ForeignKey(nameof(InventoryItem))]
    public int InventoryItemId { get; set; }

    public Member Member { get; set; } = null!;
    public InventoryItem InventoryItem { get; set; } = null!;
}
