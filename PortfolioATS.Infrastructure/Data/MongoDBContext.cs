using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using PortfolioATS.Core.Entities;

namespace PortfolioATS.Infrastructure.Data
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoDBSettings _settings;
        private bool _indexesCreated = false;
        private readonly object _lockObject = new object();

        static MongoDBContext()
        {
            // Configurar convenções do MongoDB
            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String)
            };
            ConventionRegistry.Register("MyConventions", pack, t => true);

            // Configurar serialização de Guid
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        }

        public MongoDBContext(IOptions<MongoDBSettings> settings)
        {
            _settings = settings.Value;

            // Configurar as settings do MongoClient
            var mongoClientSettings = MongoClientSettings.FromConnectionString(_settings.ConnectionString);
            mongoClientSettings.GuidRepresentation = GuidRepresentation.Standard;

            var client = new MongoClient(mongoClientSettings);
            _database = client.GetDatabase(_settings.DatabaseName);

            // Criar índices para melhor performance
            CreateIndexes();
        }

        // Collections
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Profile> Profiles => _database.GetCollection<Profile>("Profiles");

        public async Task EnsureIndexesCreatedAsync()
        {
            if (_indexesCreated) return;

            lock (_lockObject)
            {
                if (_indexesCreated) return;

                try
                {
                    CreateIndexes();
                    _indexesCreated = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao criar índices: {ex.Message}");
                }
            }
        }

        private void CreateIndexes()
        {
            try
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
            catch (Exception ex)
            {
                // Log do erro (em produção, usar ILogger)
                Console.WriteLine($"Erro ao criar índices: {ex.Message}");
            }
        }
    }
}