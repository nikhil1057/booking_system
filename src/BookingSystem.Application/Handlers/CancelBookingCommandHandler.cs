using BookingSystem.Core.Interfaces;
using MediatR;
using BookingSystem.Application.Commands;
using BookingSystem.Core.Constants;
using BookingSystem.Core.Models;

namespace BookingSystem.Application.Handlers
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, CancelBookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public CancelBookingCommandHandler(IMemberRepository memberRepository, IBookingRepository bookingRepository, IInventoryRepository inventoryRepository)
        {
            _bookingRepository = bookingRepository;
            _memberRepository = memberRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<CancelBookingResponse> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            // Get booking by reference number
            var booking = await _bookingRepository.GetByReferenceNumberAsync(request.ReferenceNumber);
            if (booking == null)
            {
                return new CancelBookingResponse
                {
                    Success = false,
                    ErrorMessage = $"Booking with reference number {request.ReferenceNumber} not found."
                };
            }

            var member = await _memberRepository.GetByIdAsync(booking.MemberId);

            if (member == null)
            {
                return new CancelBookingResponse
                {
                    Success = false,
                    ErrorMessage = $"Member with booking not found."
                };
            }

            // Get inventory item by id
            var inventoryItem = await _inventoryRepository.GetByIdAsync(booking.InventoryItemId);
            if (inventoryItem == null)
            {
                return new CancelBookingResponse
                {
                    Success = false,
                    ErrorMessage = $"Inventory item with booking not found."
                };
            }

            // Check if booking is already cancelled
            if (booking.IsCancelled)
            {
                return new CancelBookingResponse
                {
                    Success = false,
                    ErrorMessage = $"Booking with reference number {request.ReferenceNumber} is already cancelled."
                };
            }

            // Cancel booking
            booking.IsCancelled = true;
            booking.CancellationDate = DateTime.UtcNow;
            member.BookingCount -= 1;
            inventoryItem.RemainingCount += 1;

            await _bookingRepository.UpdateAsync(booking);
            await _memberRepository.UpdateAsync(member);
            await _inventoryRepository.UpdateAsync(inventoryItem);

            return new CancelBookingResponse
            {
                Success = true
            };
        }
    }
}
