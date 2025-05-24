using BookingSystem.Core.Models;

namespace BookingSystem.Core.Interfaces
{

    public interface IInventoryRepository : IRepository<InventoryItem>
    {
        Task<InventoryItem?> GetByNameAsync(string name);
    }
}
