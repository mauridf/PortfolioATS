using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface ISkillRepository : IEmbeddedRepository<Skill>
    {
        Task<IEnumerable<Skill>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Skill>> GetByCategoryAsync(string category, string userId); // Atualizado
        Task<IEnumerable<Skill>> GetByUserIdAndCategoryAsync(string userId, string category);
        Task<Skill> AddToProfileAsync(string userId, Skill entity);
        Task<bool> UpdateInProfileAsync(string userId, string entityId, Skill entity);
        Task<bool> DeleteFromProfileAsync(string userId, string entityId);
    }
}