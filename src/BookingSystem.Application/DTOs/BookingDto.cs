namespace BookingSystem.Application.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime? CancellationDate { get; set; }
    public MemberDto Member { get; set; } = null!;
    public InventoryItemDto InventoryItem { get; set; } = null!;
}
