using MongoDB.Bson.Serialization.Attributes;

namespace PortfolioATS.Core.Entities
{
    public class Language
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Proficiency { get; set; } = string.Empty;
    }
}