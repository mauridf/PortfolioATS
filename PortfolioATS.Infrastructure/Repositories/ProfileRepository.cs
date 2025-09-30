using MongoDB.Driver;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Infrastructure.Data;

namespace PortfolioATS.Infrastructure.Repositories
{
    public class ProfileRepository : BaseRepository<Profile>, IProfileRepository
    {
        public ProfileRepository(MongoDBContext context) : base(context.Profiles) { }

        public async Task<Profile> GetByUserIdAsync(string userId)
        {
            return await _collection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Profile> GetByEmailAsync(string email)
        {
            return await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> ProfileExistsForUserAsync(string userId)
        {
            return await _collection.Find(x => x.UserId == userId).AnyAsync();
        }
    }
}