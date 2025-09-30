namespace PortfolioATS.Core.Entities
{
    public class Education
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string FieldOfStudy { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public string? Description { get; set; }
    }
}