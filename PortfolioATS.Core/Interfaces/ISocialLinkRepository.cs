using PortfolioATS.Core.Entities;

namespace PortfolioATS.Core.Interfaces
{
    public interface ISocialLinkRepository : IEmbeddedRepository<SocialLink>
    {
        Task<IEnumerable<SocialLink>> GetByPlatformAsync(string platform);
    }
}