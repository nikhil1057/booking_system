namespace BookingSystem.Application.DTOs;

public class InventoryItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
    public int RemainingCount { get; set; }
}
