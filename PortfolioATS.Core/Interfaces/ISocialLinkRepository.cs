using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface ISocialLinkRepository : IEmbeddedRepository<SocialLink>
    {
        Task<IEnumerable<SocialLink>> GetByUserIdAsync(string userId);
        Task<IEnumerable<SocialLink>> GetByPlatformAsync(string platform, string userId); // Atualizado
        Task<SocialLink> AddToProfileAsync(string userId, SocialLink entity);
        Task<bool> UpdateInProfileAsync(string userId, string entityId, SocialLink entity);
        Task<bool> DeleteFromProfileAsync(string userId, string entityId);
    }
}