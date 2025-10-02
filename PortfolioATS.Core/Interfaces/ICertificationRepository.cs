using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface ICertificationRepository : IEmbeddedRepository<Certification>
    {
        Task<IEnumerable<Certification>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Certification>> GetByIssuingOrganizationAsync(string organization, string userId); // Atualizado
        Task<IEnumerable<Certification>> GetExpiredCertificationsAsync(string userId);
        Task<Certification> AddToProfileAsync(string userId, Certification entity);
        Task<bool> UpdateInProfileAsync(string userId, string entityId, Certification entity);
        Task<bool> DeleteFromProfileAsync(string userId, string entityId);
    }
}