using MongoDB.Driver;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Infrastructure.Data;

namespace PortfolioATS.Infrastructure.Repositories
{
    public class LanguageRepository : BaseEmbeddedRepository<Language>, ILanguageRepository
    {
        public LanguageRepository(MongoDBContext context) : base(context) { }

        public async Task<IEnumerable<Language>> GetByUserIdAsync(string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile?.Languages ?? new List<Language>();
        }

        public async Task<IEnumerable<Language>> GetByProficiencyAsync(string proficiency)
        {
            var profiles = await _profileCollection.Find(_ => true).ToListAsync();
            return profiles.SelectMany(p => p.Languages.Where(l => l.Proficiency == proficiency));
        }

        public async Task<Language> AddToProfileAsync(string userId, Language entity)
        {
            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.Push(p => p.Languages, entity);

            await _profileCollection.UpdateOneAsync(filter, update);
            return entity;
        }

        public async Task<bool> UpdateInProfileAsync(string userId, string entityId, Language entity)
        {
            var filter = Builders<Profile>.Filter.And(
                Builders<Profile>.Filter.Eq(p => p.UserId, userId),
                Builders<Profile>.Filter.ElemMatch(p => p.Languages, l => l.Id == entityId)
            );

            var update = Builders<Profile>.Update.Set("Languages.$", entity);
            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteFromProfileAsync(string userId, string entityId)
        {
            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.PullFilter(p => p.Languages, l => l.Id == entityId);

            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}