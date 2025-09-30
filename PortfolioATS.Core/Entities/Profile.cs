namespace PortfolioATS.Core.Entities
{
    public class Profile : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        // Dados Pessoais
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ProfessionalSummary { get; set; } = string.Empty;

        // Relacionamentos
        public List<SocialLink> SocialLinks { get; set; } = new();
        public List<Experience> Experiences { get; set; } = new();
        public List<Skill> Skills { get; set; } = new(); // Todas as skills do usuário
        public List<Education> Educations { get; set; } = new();
        public List<Certification> Certifications { get; set; } = new();
        public List<Language> Languages { get; set; } = new();
    }
}