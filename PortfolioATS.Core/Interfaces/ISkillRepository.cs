using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface ISkillRepository : IEmbeddedRepository<Skill>
    {
        Task<IEnumerable<Skill>> GetByCategoryAsync(string category);
        Task<IEnumerable<Skill>> GetByUserIdAndCategoryAsync(string userId, string category);
    }
}