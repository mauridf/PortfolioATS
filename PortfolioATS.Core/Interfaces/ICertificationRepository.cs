using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface ICertificationRepository : IEmbeddedRepository<Certification>
    {
        Task<IEnumerable<Certification>> GetByIssuingOrganizationAsync(string organization);
        Task<IEnumerable<Certification>> GetExpiredCertificationsAsync(string userId);
    }
}