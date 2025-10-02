using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface ILanguageRepository : IEmbeddedRepository<Language>
    {
        Task<IEnumerable<Language>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Language>> GetByProficiencyAsync(string proficiency, string userId); // Atualizado
        Task<Language> AddToProfileAsync(string userId, Language entity);
        Task<bool> UpdateInProfileAsync(string userId, string entityId, Language entity);
        Task<bool> DeleteFromProfileAsync(string userId, string entityId);
    }
}