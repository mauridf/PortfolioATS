namespace PortfolioATS.Core.DTOs
{
    public class DashboardDto
    {
        public ProfileSummaryDto ProfileSummary { get; set; } = new();
        public StatisticsDto Statistics { get; set; } = new();
        public List<QuickActionDto> QuickActions { get; set; } = new();
        public RecentActivityDto RecentActivity { get; set; } = new();
        public AtsScoreDto AtsScore { get; set; } = new();
    }

    public class ProfileSummaryDto
    {
        public string FullName { get; set; } = string.Empty;
        public string ProfessionalTitle { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ProfessionalSummary { get; set; } = string.Empty;
        public string ProfileCompletion { get; set; } = "0%";
    }

    public class StatisticsDto
    {
        public int TotalExperiences { get; set; }
        public int TotalSkills { get; set; }
        public int TotalEducations { get; set; }
        public int TotalCertifications { get; set; }
        public int TotalLanguages { get; set; }
        public int CurrentExperiences { get; set; }
        public int SkillsByCategory { get; set; }
    }

    public class QuickActionDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // "add-experience", "complete-profile", etc.
        public string Icon { get; set; } = string.Empty; // Para o frontend
        public int Priority { get; set; }
    }

    public class RecentActivityDto
    {
        public List<ActivityItemDto> Activities { get; set; } = new();
    }

    public class ActivityItemDto
    {
        public string Type { get; set; } = string.Empty; // "experience-added", "profile-updated"
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string EntityId { get; set; } = string.Empty;
    }

    public class AtsScoreDto
    {
        public int Score { get; set; } // 0-100
        public string Level { get; set; } = "Iniciante"; // Iniciante, Intermediário, Avançado, Expert
        public List<string> Suggestions { get; set; } = new();
    }
}