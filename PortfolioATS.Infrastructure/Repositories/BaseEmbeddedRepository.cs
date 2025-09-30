using MongoDB.Driver;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Infrastructure.Data;

namespace PortfolioATS.Infrastructure.Repositories
{
    public abstract class BaseEmbeddedRepository<T>
    {
        protected readonly IMongoCollection<Profile> _profileCollection;

        protected BaseEmbeddedRepository(MongoDBContext context)
        {
            _profileCollection = context.Profiles;
        }

        protected string GetCollectionName<TEntity>()
        {
            return typeof(TEntity).Name.ToLower() + "s";
        }
    }
}