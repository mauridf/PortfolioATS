using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface IEducationRepository : IEmbeddedRepository<Education>
    {
        Task<IEnumerable<Education>> GetByInstitutionAsync(string institution);
    }
}