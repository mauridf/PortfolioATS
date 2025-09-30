using MongoDB.Driver;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Infrastructure.Data;

namespace PortfolioATS.Infrastructure.Repositories
{
    public class EducationRepository : BaseEmbeddedRepository<Education>, IEducationRepository
    {
        public EducationRepository(MongoDBContext context) : base(context) { }

        public async Task<IEnumerable<Education>> GetByUserIdAsync(string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile?.Educations ?? new List<Education>();
        }

        public async Task<IEnumerable<Education>> GetByInstitutionAsync(string institution)
        {
            var profiles = await _profileCollection.Find(_ => true).ToListAsync();
            return profiles.SelectMany(p => p.Educations.Where(e => e.Institution.Contains(institution)));
        }

        public async Task<Education> AddToProfileAsync(string userId, Education entity)
        {
            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.Push(p => p.Educations, entity);

            await _profileCollection.UpdateOneAsync(filter, update);
            return entity;
        }

        public async Task<bool> UpdateInProfileAsync(string userId, string entityId, Education entity)
        {
            var filter = Builders<Profile>.Filter.And(
                Builders<Profile>.Filter.Eq(p => p.UserId, userId),
                Builders<Profile>.Filter.ElemMatch(p => p.Educations, e => e.Id == entityId)
            );

            var update = Builders<Profile>.Update.Set("Educations.$", entity);
            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteFromProfileAsync(string userId, string entityId)
        {
            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.PullFilter(p => p.Educations, e => e.Id == entityId);

            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}