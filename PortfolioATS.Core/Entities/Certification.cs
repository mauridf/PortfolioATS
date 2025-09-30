using MongoDB.Bson.Serialization.Attributes;

namespace PortfolioATS.Core.Entities
{
    public class Certification
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;
        public string IssuingOrganization { get; set; } = string.Empty;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime IssueDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? ExpirationDate { get; set; }

        public string? CredentialId { get; set; }
        public string? CredentialUrl { get; set; }
    }
}