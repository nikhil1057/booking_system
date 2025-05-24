using BookingSystem.Core.Interfaces;
using MediatR;
using BookingSystem.Application.Commands;
using BookingSystem.Core.Constants;
using BookingSystem.Core.Models;

namespace BookingSystem.Application.Handlers
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponse>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IBookingRepository _bookingRepository;

        public CreateBookingCommandHandler(
            IMemberRepository memberRepository,
            IInventoryRepository inventoryRepository,
            IBookingRepository bookingRepository)
        {
            _memberRepository = memberRepository;
            _inventoryRepository = inventoryRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // Get member by email
            var member = await _memberRepository.GetByEmailAsync(request.MemberEmail);
            if (member == null)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorMessage = $"Member with email {request.MemberEmail} not found."
                };
            }

            // Check if member has reached maximum bookings
            if (member.ActiveBookings.Count >= BookingConstants.MAX_BOOKINGS || member.BookingCount >= BookingConstants.MAX_BOOKINGS)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorMessage = $"Member has reached the maximum number of bookings ({BookingConstants.MAX_BOOKINGS})."
                };
            }

            // Get inventory item by name
            var inventoryItem = await _inventoryRepository.GetByNameAsync(request.InventoryItemName);
            if (inventoryItem == null)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorMessage = $"Inventory item '{request.InventoryItemName}' not found."
                };
            }

            // Check if the inventory item is available
            if (!inventoryItem.IsAvailable)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorMessage = $"Inventory item '{request.InventoryItemName}' is not available."
                };
            }

            // Create booking
            var booking = new Booking
            {
                ReferenceNumber = GenerateReferenceNumber(),
                BookingDate = DateTime.UtcNow,
                IsCancelled = false,
                MemberId = member.Id,
                InventoryItemId = inventoryItem.Id
            };

            member.BookingCount += 1;
            inventoryItem.RemainingCount -= 1;
            await _memberRepository.UpdateAsync(member);
            await _inventoryRepository.UpdateAsync(inventoryItem);
            await _bookingRepository.AddAsync(booking);

            return new BookingResponse
            {
                Success = true,
                BookingReference = booking.ReferenceNumber
            };
        }

        private string GenerateReferenceNumber()
        {
            // Generate a unique reference number (e.g., BK-{timestamp}-{random 4 digits})
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = new Random().Next(1000, 9999);
            return $"BK-{timestamp}-{random}";
        }
    }
}
