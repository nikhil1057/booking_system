using MediatR;

namespace BookingSystem.Application.Commands
{
    public record CreateBookingCommand(string MemberEmail, string InventoryItemName) : IRequest<BookingResponse>;

    public class BookingResponse
    {
        public bool Success { get; set; }
        public string? BookingReference { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
