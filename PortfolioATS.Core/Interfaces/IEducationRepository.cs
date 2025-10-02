using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface IEducationRepository : IEmbeddedRepository<Education>
    {
        Task<IEnumerable<Education>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Education>> GetByInstitutionAsync(string institution, string userId); // Atualizado
        Task<Education> AddToProfileAsync(string userId, Education entity);
        Task<bool> UpdateInProfileAsync(string userId, string entityId, Education entity);
        Task<bool> DeleteFromProfileAsync(string userId, string entityId);
    }
}