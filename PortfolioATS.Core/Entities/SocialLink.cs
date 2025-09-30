using MongoDB.Bson.Serialization.Attributes;

namespace PortfolioATS.Core.Entities
{
    public class SocialLink
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Platform { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Username { get; set; }
    }
}