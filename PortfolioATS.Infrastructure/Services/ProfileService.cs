using PortfolioATS.Core.DTOs;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;

namespace PortfolioATS.Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IExperienceRepository _experienceRepository;

        public ProfileService(IProfileRepository profileRepository, IExperienceRepository experienceRepository)
        {
            _profileRepository = profileRepository;
            _experienceRepository = experienceRepository;
        }

        public async Task<ProfileDto> GetProfileByUserIdAsync(string userId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                throw new ArgumentException("Perfil não encontrado.");
            }

            // Carregar experiências com skills completas
            var experiencesWithSkills = await _experienceRepository.GetByUserIdAsync(userId);
            profile.Experiences = experiencesWithSkills.ToList();

            return MapToDto(profile);
        }

        public async Task<ProfileDto> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                throw new ArgumentException("Perfil não encontrado.");
            }

            profile.FullName = request.FullName;
            profile.Phone = request.Phone;
            profile.Location = request.Location;
            profile.ProfessionalSummary = request.ProfessionalSummary;
            profile.UpdatedAt = DateTime.UtcNow;

            var updatedProfile = await _profileRepository.UpdateAsync(profile);
            return MapToDto(updatedProfile);
        }

        public async Task<bool> ProfileExistsAsync(string userId)
        {
            return await _profileRepository.ProfileExistsForUserAsync(userId);
        }

        private ProfileDto MapToDto(Profile profile)
        {
            return new ProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FullName = profile.FullName,
                Email = profile.Email,
                Phone = profile.Phone,
                Location = profile.Location,
                ProfessionalSummary = profile.ProfessionalSummary,
                SocialLinks = profile.SocialLinks.Select(sl => new SocialLinkDto
                {
                    Id = sl.Id,
                    Platform = sl.Platform,
                    Url = sl.Url,
                    Username = sl.Username
                }).ToList(),
                Experiences = profile.Experiences.Select(e => new ExperienceDto
                {
                    Id = e.Id,
                    Company = e.Company,
                    Position = e.Position,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    IsCurrent = e.IsCurrent,
                    Description = e.Description,
                    EmploymentType = e.EmploymentType,
                    SkillIds = e.SkillIds,
                    Skills = e.Skills.Select(s => new SkillDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Category = s.Category,
                        Level = s.Level,
                        YearsOfExperience = s.YearsOfExperience
                    }).ToList()
                }).ToList(),
                Skills = profile.Skills.Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category,
                    Level = s.Level,
                    YearsOfExperience = s.YearsOfExperience
                }).ToList(),
                Educations = profile.Educations.Select(e => new EducationDto
                {
                    Id = e.Id,
                    Institution = e.Institution,
                    Degree = e.Degree,
                    FieldOfStudy = e.FieldOfStudy,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    IsCompleted = e.IsCompleted,
                    Description = e.Description
                }).ToList(),
                Certifications = profile.Certifications.Select(c => new CertificationDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IssuingOrganization = c.IssuingOrganization,
                    IssueDate = c.IssueDate,
                    ExpirationDate = c.ExpirationDate,
                    CredentialId = c.CredentialId,
                    CredentialUrl = c.CredentialUrl
                }).ToList(),
                Languages = profile.Languages.Select(l => new LanguageDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    Proficiency = l.Proficiency
                }).ToList()
            };
        }
    }
}