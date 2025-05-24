using BookingSystem.Core.Interfaces;
using BookingSystem.Core.Models;
using BookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Booking?> GetByReferenceNumberAsync(string referenceNumber)
        {
            return await _context.Bookings
                .Include(b => b.Member)
                .Include(b => b.InventoryItem)
                .FirstOrDefaultAsync(b => b.ReferenceNumber == referenceNumber);
        }

        public async Task<IEnumerable<Booking>> GetByMemberIdAsync(int memberId)
        {
            return await _context.Bookings
                .Include(b => b.InventoryItem)
                .Where(b => b.MemberId == memberId)
                .ToListAsync();
        }
    }
}
