using BookingSystem.Core.Interfaces;
using BookingSystem.Core.Models;
using BookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Member?> GetByEmailAsync(string email)
        {
            return await _context.Members
                .Include(m => m.Bookings)
                .FirstOrDefaultAsync(m => m.Email == email);
        }
    }
}
