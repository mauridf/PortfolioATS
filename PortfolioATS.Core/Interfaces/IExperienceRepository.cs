using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface IExperienceRepository : IEmbeddedRepository<Experience>
    {
        Task<IEnumerable<Experience>> GetByCompanyAsync(string company);
        Task<IEnumerable<Experience>> GetCurrentExperiencesAsync(string userId);
    }
}