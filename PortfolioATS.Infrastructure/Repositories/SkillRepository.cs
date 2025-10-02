using MongoDB.Driver;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Infrastructure.Data;

namespace PortfolioATS.Infrastructure.Repositories
{
    public class SkillRepository : BaseEmbeddedRepository<Skill>, ISkillRepository
    {
        public SkillRepository(MongoDBContext context) : base(context) { }

        public async Task<IEnumerable<Skill>> GetByUserIdAsync(string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile?.Skills ?? new List<Skill>();
        }

        public async Task<IEnumerable<Skill>> GetByCategoryAsync(string category, string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile?.Skills.Where(s => s.Category == category) ?? new List<Skill>();
        }

        public async Task<IEnumerable<Skill>> GetByUserIdAndCategoryAsync(string userId, string category)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile?.Skills.Where(s => s.Category == category) ?? new List<Skill>();
        }

        public async Task<Skill> AddToProfileAsync(string userId, Skill entity)
        {
            entity.UserId = userId;

            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.Push(p => p.Skills, entity);

            await _profileCollection.UpdateOneAsync(filter, update);
            return entity;
        }

        public async Task<bool> UpdateInProfileAsync(string userId, string entityId, Skill entity)
        {
            entity.UserId = userId;

            var filter = Builders<Profile>.Filter.And(
                Builders<Profile>.Filter.Eq(p => p.UserId, userId),
                Builders<Profile>.Filter.ElemMatch(p => p.Skills, s => s.Id == entityId)
            );

            var update = Builders<Profile>.Update.Set("Skills.$", entity);
            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteFromProfileAsync(string userId, string entityId)
        {
            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.PullFilter(p => p.Skills, s => s.Id == entityId);

            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}