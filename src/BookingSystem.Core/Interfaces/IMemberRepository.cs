using BookingSystem.Core.Models;

namespace BookingSystem.Core.Interfaces
{

    public interface IMemberRepository : IRepository<Member>
    {
        Task<Member?> GetByEmailAsync(string email);
    }
}
