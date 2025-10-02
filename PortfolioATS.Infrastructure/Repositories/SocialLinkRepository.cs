using MongoDB.Driver;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Infrastructure.Data;

namespace PortfolioATS.Infrastructure.Repositories
{
    public class SocialLinkRepository : BaseEmbeddedRepository<SocialLink>, ISocialLinkRepository
    {
        public SocialLinkRepository(MongoDBContext context) : base(context) { }

        public async Task<IEnumerable<SocialLink>> GetByUserIdAsync(string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile?.SocialLinks ?? new List<SocialLink>();
        }

        public async Task<IEnumerable<SocialLink>> GetByPlatformAsync(string platform, string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile?.SocialLinks.Where(sl => sl.Platform == platform) ?? new List<SocialLink>();
        }

        public async Task<SocialLink> AddToProfileAsync(string userId, SocialLink entity)
        {
            // Garantir que o UserId está correto
            entity.UserId = userId;

            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.Push(p => p.SocialLinks, entity);

            await _profileCollection.UpdateOneAsync(filter, update);
            return entity;
        }

        public async Task<bool> UpdateInProfileAsync(string userId, string entityId, SocialLink entity)
        {
            // Garantir que o UserId está correto
            entity.UserId = userId;

            var filter = Builders<Profile>.Filter.And(
                Builders<Profile>.Filter.Eq(p => p.UserId, userId),
                Builders<Profile>.Filter.ElemMatch(p => p.SocialLinks, sl => sl.Id == entityId)
            );

            var update = Builders<Profile>.Update.Set("SocialLinks.$", entity);
            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteFromProfileAsync(string userId, string entityId)
        {
            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.PullFilter(p => p.SocialLinks, sl => sl.Id == entityId);

            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}