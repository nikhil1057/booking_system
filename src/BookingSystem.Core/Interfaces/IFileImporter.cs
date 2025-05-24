using BookingSystem.Core.Models;

namespace BookingSystem.Core.Interfaces
{

    public interface IFileImporter
    {
        Task ImportMembersAsync(Stream fileStream);
        Task ImportInventoryAsync(Stream fileStream);
    }
}
