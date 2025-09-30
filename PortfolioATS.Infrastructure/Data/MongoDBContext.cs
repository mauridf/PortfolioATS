using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PortfolioATS.Core.Entities;

namespace PortfolioATS.Infrastructure.Data
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoDBSettings _settings;

        public MongoDBContext(IOptions<MongoDBSettings> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);

            // Criar índices para melhor performance
            CreateIndexes();
        }

        // Collections
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Profile> Profiles => _database.GetCollection<Profile>("Profiles");

        private void CreateIndexes()
        {
            // Índice único para email na collection Users
            var userEmailIndex = Builders<User>.IndexKeys.Ascending(u => u.Email);
            Users.Indexes.CreateOne(new CreateIndexModel<User>(userEmailIndex, new CreateIndexOptions { Unique = true }));

            // Índice para userId na collection Profiles
            var profileUserIdIndex = Builders<Profile>.IndexKeys.Ascending(p => p.UserId);
            Profiles.Indexes.CreateOne(new CreateIndexModel<Profile>(profileUserIdIndex, new CreateIndexOptions { Unique = true }));

            // Índice para email na collection Profiles
            var profileEmailIndex = Builders<Profile>.IndexKeys.Ascending(p => p.Email);
            Profiles.Indexes.CreateOne(new CreateIndexModel<Profile>(profileEmailIndex));
        }
    }
}