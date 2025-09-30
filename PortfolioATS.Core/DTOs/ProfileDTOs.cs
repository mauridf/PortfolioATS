namespace PortfolioATS.Core.DTOs
{
    public class ProfileDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ProfessionalSummary { get; set; } = string.Empty;

        public List<SocialLinkDto> SocialLinks { get; set; } = new();
        public List<ExperienceDto> Experiences { get; set; } = new();
        public List<SkillDto> Skills { get; set; } = new();
        public List<EducationDto> Educations { get; set; } = new();
        public List<CertificationDto> Certifications { get; set; } = new();
        public List<LanguageDto> Languages { get; set; } = new();
    }

    public class UpdateProfileRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ProfessionalSummary { get; set; } = string.Empty;
    }
}