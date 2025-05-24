using MediatR;

namespace BookingSystem.Application.Commands
{
    public record CancelBookingCommand(string ReferenceNumber) : IRequest<CancelBookingResponse>;

    public class CancelBookingResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
