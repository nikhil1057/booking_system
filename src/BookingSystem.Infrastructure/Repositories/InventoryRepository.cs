using BookingSystem.Core.Interfaces;
using BookingSystem.Core.Models;
using BookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories
{
    public class InventoryRepository : Repository<InventoryItem>, IInventoryRepository
    {
        public InventoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<InventoryItem?> GetByNameAsync(string name)
        {
            return await _context.InventoryItems
                .Include(i => i.Bookings)
                .FirstOrDefaultAsync(i => i.Name == name);
        }
    }
}
