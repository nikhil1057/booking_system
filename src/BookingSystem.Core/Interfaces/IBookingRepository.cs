using BookingSystem.Core.Models;

namespace BookingSystem.Core.Interfaces
{

    public interface IBookingRepository : IRepository<Booking>
    {
        Task<Booking?> GetByReferenceNumberAsync(string referenceNumber);
        Task<IEnumerable<Booking>> GetByMemberIdAsync(int memberId);
    }
}
