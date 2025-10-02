using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface IExperienceRepository : IEmbeddedRepository<Experience>
    {
        Task<IEnumerable<Experience>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Experience>> GetByCompanyAsync(string company, string userId); // Atualizado
        Task<IEnumerable<Experience>> GetCurrentExperiencesAsync(string userId);
        Task<Experience> AddToProfileAsync(string userId, Experience entity);
        Task<bool> UpdateInProfileAsync(string userId, string entityId, Experience entity);
        Task<bool> DeleteFromProfileAsync(string userId, string entityId);
    }
}