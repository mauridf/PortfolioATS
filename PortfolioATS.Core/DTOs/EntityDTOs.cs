namespace PortfolioATS.Core.DTOs
{
    // SocialLink
    public class SocialLinkDto
    {
        public string Id { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Username { get; set; }
    }

    public class CreateSocialLinkRequest
    {
        public string Platform { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Username { get; set; }
    }

    // Experience
    public class ExperienceDto
    {
        public string Id { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public string Description { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public List<string> SkillIds { get; set; } = new();
        public List<SkillDto> Skills { get; set; } = new();
    }

    public class CreateExperienceRequest
    {
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public string Description { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public List<string> SkillIds { get; set; } = new();
    }

    // Skill
    public class SkillDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Level { get; set; } = "Intermediário";
        public int YearsOfExperience { get; set; }
    }

    public class CreateSkillRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Level { get; set; } = "Intermediário";
        public int YearsOfExperience { get; set; }
    }

    // Education
    public class EducationDto
    {
        public string Id { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string FieldOfStudy { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public string? Description { get; set; }
    }

    public class CreateEducationRequest
    {
        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string FieldOfStudy { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public string? Description { get; set; }
    }

    // Certification
    public class CertificationDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string IssuingOrganization { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? CredentialId { get; set; }
        public string? CredentialUrl { get; set; }
    }

    public class CreateCertificationRequest
    {
        public string Name { get; set; } = string.Empty;
        public string IssuingOrganization { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? CredentialId { get; set; }
        public string? CredentialUrl { get; set; }
    }

    // Language
    public class LanguageDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Proficiency { get; set; } = string.Empty;
    }

    public class CreateLanguageRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Proficiency { get; set; } = string.Empty;
    }
}