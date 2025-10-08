using PortfolioATS.Core.DTOs;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;

namespace PortfolioATS.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IExperienceRepository _experienceRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly ICertificationRepository _certificationRepository;
        private readonly ILanguageRepository _languageRepository;

        public DashboardService(
            IProfileRepository profileRepository,
            IExperienceRepository experienceRepository,
            ISkillRepository skillRepository,
            IEducationRepository educationRepository,
            ICertificationRepository certificationRepository,
            ILanguageRepository languageRepository)
        {
            _profileRepository = profileRepository;
            _experienceRepository = experienceRepository;
            _skillRepository = skillRepository;
            _educationRepository = educationRepository;
            _certificationRepository = certificationRepository;
            _languageRepository = languageRepository;
        }

        public async Task<DashboardDto> GetDashboardDataAsync(string userId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                throw new ArgumentException("Perfil não encontrado.");
            }

            var dashboard = new DashboardDto
            {
                ProfileSummary = await GetProfileSummaryAsync(profile),
                Statistics = await GetStatisticsAsync(userId),
                QuickActions = await GetQuickActionsAsync(profile, userId),
                RecentActivity = await GetRecentActivityAsync(profile),
                AtsScore = await CalculateAtsScoreAsync(userId)
            };

            return dashboard;
        }

        public async Task<int> CalculateProfileCompletionAsync(string userId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null) return 0;

            int totalFields = 10; // Ajuste conforme os campos obrigatórios
            int completedFields = 0;

            // Campos básicos do perfil
            if (!string.IsNullOrEmpty(profile.FullName)) completedFields++;
            if (!string.IsNullOrEmpty(profile.Email)) completedFields++;
            if (!string.IsNullOrEmpty(profile.Phone)) completedFields++;
            if (!string.IsNullOrEmpty(profile.Location)) completedFields++;
            if (!string.IsNullOrEmpty(profile.ProfessionalSummary)) completedFields++;

            // Seções preenchidas
            if (profile.Experiences.Any()) completedFields++;
            if (profile.Skills.Any()) completedFields++;
            if (profile.Educations.Any()) completedFields++;
            if (profile.SocialLinks.Any()) completedFields++;
            if (profile.Languages.Any()) completedFields++;

            return (completedFields * 100) / totalFields;
        }

        public async Task<AtsScoreDto> CalculateAtsScoreAsync(string userId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                return new AtsScoreDto { Score = 0, Level = "Iniciante", Suggestions = new List<string> { "Complete seu perfil básico" } };
            }

            int score = 0;
            var suggestions = new List<string>();

            // Critérios ATS (Applicant Tracking System)
            if (!string.IsNullOrEmpty(profile.ProfessionalSummary) && profile.ProfessionalSummary.Length > 50)
                score += 15;
            else
                suggestions.Add("Adicione um resumo profissional mais detalhado");

            if (profile.Experiences.Count >= 2)
                score += 25;
            else
                suggestions.Add("Adicione pelo menos 2 experiências profissionais");

            if (profile.Skills.Count >= 5)
                score += 20;
            else
                suggestions.Add("Inclua pelo menos 5 habilidades técnicas");

            if (profile.Educations.Any())
                score += 15;
            else
                suggestions.Add("Adicione sua formação acadêmica");

            if (profile.Certifications.Any())
                score += 10;
            else
                suggestions.Add("Certificações aumentam sua pontuação");

            if (profile.Languages.Any())
                score += 10;
            else
                suggestions.Add("Idiomas são valorizados");

            if (profile.SocialLinks.Any(s => s.Platform == "LinkedIn"))
                score += 5;

            // Determinar nível
            string level = score switch
            {
                >= 90 => "Expert",
                >= 70 => "Avançado",
                >= 50 => "Intermediário",
                _ => "Iniciante"
            };

            return new AtsScoreDto
            {
                Score = score,
                Level = level,
                Suggestions = suggestions
            };
        }

        private async Task<ProfileSummaryDto> GetProfileSummaryAsync(Profile profile)
        {
            var completion = await CalculateProfileCompletionAsync(profile.UserId);

            // Carregar experiências com skills para obter a posição atual
            var experiences = await _experienceRepository.GetByUserIdAsync(profile.UserId);
            var currentPosition = experiences
                .Where(e => e.IsCurrent)
                .OrderByDescending(e => e.StartDate)
                .FirstOrDefault()?.Position ?? "Profissional";

            return new ProfileSummaryDto
            {
                FullName = profile.FullName,
                ProfessionalTitle = currentPosition,
                Location = profile.Location,
                Email = profile.Email,
                Phone = profile.Phone,
                ProfessionalSummary = profile.ProfessionalSummary,
                ProfileCompletion = $"{completion}%"
            };
        }

        private async Task<StatisticsDto> GetStatisticsAsync(string userId)
        {
            var experiences = await _experienceRepository.GetByUserIdAsync(userId);
            var skills = await _skillRepository.GetByUserIdAsync(userId);
            var educations = await _educationRepository.GetByUserIdAsync(userId);
            var certifications = await _certificationRepository.GetByUserIdAsync(userId);
            var languages = await _languageRepository.GetByUserIdAsync(userId);

            return new StatisticsDto
            {
                TotalExperiences = experiences.Count(),
                TotalSkills = skills.Count(),
                TotalEducations = educations.Count(),
                TotalCertifications = certifications.Count(),
                TotalLanguages = languages.Count(),
                CurrentExperiences = experiences.Count(e => e.IsCurrent),
                SkillsByCategory = skills.GroupBy(s => s.Category).Count()
            };
        }

        private async Task<List<QuickActionDto>> GetQuickActionsAsync(Profile profile, string userId)
        {
            var actions = new List<QuickActionDto>();
            var completion = await CalculateProfileCompletionAsync(userId);

            if (completion < 50)
            {
                actions.Add(new QuickActionDto
                {
                    Title = "Complete seu perfil",
                    Description = "Adicione informações básicas para melhorar seu currículo",
                    Action = "complete-profile",
                    Icon = "user",
                    Priority = 1
                });
            }

            if (!profile.Experiences.Any())
            {
                actions.Add(new QuickActionDto
                {
                    Title = "Adicionar primeira experiência",
                    Description = "Comece com sua experiência profissional mais recente",
                    Action = "add-experience",
                    Icon = "briefcase",
                    Priority = 2
                });
            }

            if (profile.Skills.Count < 5)
            {
                actions.Add(new QuickActionDto
                {
                    Title = "Enriquecer habilidades",
                    Description = "Adicione pelo menos 5 habilidades técnicas",
                    Action = "add-skills",
                    Icon = "tools",
                    Priority = 3
                });
            }

            if (!profile.SocialLinks.Any(s => s.Platform == "LinkedIn"))
            {
                actions.Add(new QuickActionDto
                {
                    Title = "Vincular LinkedIn",
                    Description = "Conecte seu perfil do LinkedIn",
                    Action = "add-linkedin",
                    Icon = "linkedin",
                    Priority = 4
                });
            }

            return actions.OrderBy(a => a.Priority).ToList();
        }

        private async Task<RecentActivityDto> GetRecentActivityAsync(Profile profile)
        {
            var activities = new List<ActivityItemDto>();

            // Últimas experiências adicionadas
            var recentExperiences = profile.Experiences
                .OrderByDescending(e => e.StartDate)
                .Take(3);

            foreach (var exp in recentExperiences)
            {
                activities.Add(new ActivityItemDto
                {
                    Type = "experience-added",
                    Description = $"Experiência adicionada: {exp.Position} na {exp.Company}",
                    Date = exp.StartDate,
                    EntityId = exp.Id
                });
            }

            // Perfil atualizado
            if (profile.UpdatedAt > profile.CreatedAt.AddMinutes(5))
            {
                activities.Add(new ActivityItemDto
                {
                    Type = "profile-updated",
                    Description = "Perfil atualizado",
                    Date = profile.UpdatedAt,
                    EntityId = profile.Id
                });
            }

            return new RecentActivityDto
            {
                Activities = activities.OrderByDescending(a => a.Date).Take(5).ToList()
            };
        }
    }
}