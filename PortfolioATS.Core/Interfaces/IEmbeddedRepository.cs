using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    // Interface base para operações em entidades embedded
    public interface IEmbeddedRepository<T>
    {
        Task<IEnumerable<T>> GetByUserIdAsync(string userId);
        Task<T> AddToProfileAsync(string userId, T entity);
        Task<bool> UpdateInProfileAsync(string userId, string entityId, T entity);
        Task<bool> DeleteFromProfileAsync(string userId, string entityId);
    }
}