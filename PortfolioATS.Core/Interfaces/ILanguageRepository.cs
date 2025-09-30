using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface ILanguageRepository : IEmbeddedRepository<Language>
    {
        Task<IEnumerable<Language>> GetByProficiencyAsync(string proficiency);
    }
}