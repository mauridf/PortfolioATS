namespace PortfolioATS.Core.Entities
{
    public class Experience
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public string Description { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;

        public List<string> SkillIds { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();
    }
}