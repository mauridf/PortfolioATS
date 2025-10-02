using MongoDB.Bson.Serialization.Attributes;

namespace PortfolioATS.Core.Entities
{
    public class Experience
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime StartDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? EndDate { get; set; }

        public bool IsCurrent { get; set; }
        public string Description { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;

        public List<string> SkillIds { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();
    }
}