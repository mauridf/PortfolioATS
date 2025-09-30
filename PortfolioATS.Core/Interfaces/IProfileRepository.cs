using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface IProfileRepository : IRepository<Profile>
    {
        Task<Profile> GetByUserIdAsync(string userId);
        Task<Profile> GetByEmailAsync(string email);
        Task<bool> ProfileExistsForUserAsync(string userId);
    }
}