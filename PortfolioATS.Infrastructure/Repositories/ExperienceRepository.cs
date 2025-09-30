using MongoDB.Driver;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Infrastructure.Data;

namespace PortfolioATS.Infrastructure.Repositories
{
    public class ExperienceRepository : BaseEmbeddedRepository<Experience>, IExperienceRepository
    {
        public ExperienceRepository(MongoDBContext context) : base(context) { }

        public async Task<IEnumerable<Experience>> GetByUserIdAsync(string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile?.Experiences ?? new List<Experience>();
        }

        public async Task<IEnumerable<Experience>> GetByCompanyAsync(string company)
        {
            var profiles = await _profileCollection.Find(_ => true).ToListAsync();
            return profiles.SelectMany(p => p.Experiences.Where(e => e.Company.Contains(company)));
        }

        public async Task<IEnumerable<Experience>> GetCurrentExperiencesAsync(string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile?.Experiences.Where(e => e.IsCurrent) ?? new List<Experience>();
        }

        public async Task<Experience> AddToProfileAsync(string userId, Experience entity)
        {
            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.Push(p => p.Experiences, entity);

            await _profileCollection.UpdateOneAsync(filter, update);
            return entity;
        }

        public async Task<bool> UpdateInProfileAsync(string userId, string entityId, Experience entity)
        {
            var filter = Builders<Profile>.Filter.And(
                Builders<Profile>.Filter.Eq(p => p.UserId, userId),
                Builders<Profile>.Filter.ElemMatch(p => p.Experiences, e => e.Id == entityId)
            );

            var update = Builders<Profile>.Update.Set("Experiences.$", entity);
            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteFromProfileAsync(string userId, string entityId)
        {
            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.PullFilter(p => p.Experiences, e => e.Id == entityId);

            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}