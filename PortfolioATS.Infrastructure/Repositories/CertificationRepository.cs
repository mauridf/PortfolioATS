using MongoDB.Driver;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Infrastructure.Data;

namespace PortfolioATS.Infrastructure.Repositories
{
    public class CertificationRepository : BaseEmbeddedRepository<Certification>, ICertificationRepository
    {
        public CertificationRepository(MongoDBContext context) : base(context) { }

        public async Task<IEnumerable<Certification>> GetByUserIdAsync(string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile?.Certifications ?? new List<Certification>();
        }

        public async Task<IEnumerable<Certification>> GetByIssuingOrganizationAsync(string organization, string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile?.Certifications.Where(c => c.IssuingOrganization.Contains(organization)) ?? new List<Certification>();
        }

        public async Task<IEnumerable<Certification>> GetExpiredCertificationsAsync(string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            return profile?.Certifications.Where(c => c.ExpirationDate.HasValue && c.ExpirationDate < DateTime.UtcNow)
                   ?? new List<Certification>();
        }

        public async Task<Certification> AddToProfileAsync(string userId, Certification entity)
        {
            entity.UserId = userId;

            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.Push(p => p.Certifications, entity);

            await _profileCollection.UpdateOneAsync(filter, update);
            return entity;
        }

        public async Task<bool> UpdateInProfileAsync(string userId, string entityId, Certification entity)
        {
            entity.UserId = userId;

            var filter = Builders<Profile>.Filter.And(
                Builders<Profile>.Filter.Eq(p => p.UserId, userId),
                Builders<Profile>.Filter.ElemMatch(p => p.Certifications, c => c.Id == entityId)
            );

            var update = Builders<Profile>.Update.Set("Certifications.$", entity);
            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteFromProfileAsync(string userId, string entityId)
        {
            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.PullFilter(p => p.Certifications, c => c.Id == entityId);

            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}