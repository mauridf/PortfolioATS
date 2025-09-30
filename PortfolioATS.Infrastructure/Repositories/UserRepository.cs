using MongoDB.Driver;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Infrastructure.Data;

namespace PortfolioATS.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(MongoDBContext context) : base(context.Users) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _collection.Find(x => x.Email == email).AnyAsync();
        }
    }
}