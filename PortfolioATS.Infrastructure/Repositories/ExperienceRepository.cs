using MongoDB.Driver;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Infrastructure.Data;

namespace PortfolioATS.Infrastructure.Repositories
{
    public class ExperienceRepository : BaseEmbeddedRepository<Experience>, IExperienceRepository
    {
        private readonly ISkillRepository _skillRepository;

        public ExperienceRepository(MongoDBContext context, ISkillRepository skillRepository) : base(context)
        {
            _skillRepository = skillRepository;
        }

        public async Task<IEnumerable<Experience>> GetByUserIdAsync(string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            if (profile?.Experiences == null)
                return new List<Experience>();

            // Carregar skills completas para cada experiência
            var experiencesWithSkills = new List<Experience>();
            foreach (var experience in profile.Experiences)
            {
                experiencesWithSkills.Add(await LoadSkillsForExperience(experience, userId));
            }

            return experiencesWithSkills;
        }

        public async Task<IEnumerable<Experience>> GetByCompanyAsync(string company, string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            if (profile?.Experiences == null)
                return new List<Experience>();

            var filteredExperiences = profile.Experiences.Where(e => e.Company.Contains(company));

            // Carregar skills completas para cada experiência
            var experiencesWithSkills = new List<Experience>();
            foreach (var experience in filteredExperiences)
            {
                experiencesWithSkills.Add(await LoadSkillsForExperience(experience, userId));
            }

            return experiencesWithSkills;
        }

        public async Task<IEnumerable<Experience>> GetCurrentExperiencesAsync(string userId)
        {
            var profile = await _profileCollection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
            if (profile?.Experiences == null)
                return new List<Experience>();

            var currentExperiences = profile.Experiences.Where(e => e.IsCurrent);

            // Carregar skills completas para cada experiência
            var experiencesWithSkills = new List<Experience>();
            foreach (var experience in currentExperiences)
            {
                experiencesWithSkills.Add(await LoadSkillsForExperience(experience, userId));
            }

            return experiencesWithSkills;
        }

        public async Task<Experience> AddToProfileAsync(string userId, Experience entity)
        {
            entity.UserId = userId;

            // Carregar skills completas antes de salvar
            entity = await LoadSkillsForExperience(entity, userId);

            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.Push(p => p.Experiences, entity);

            await _profileCollection.UpdateOneAsync(filter, update);
            return entity;
        }

        public async Task<bool> UpdateInProfileAsync(string userId, string entityId, Experience entity)
        {
            entity.UserId = userId;

            // Carregar skills completas antes de atualizar
            entity = await LoadSkillsForExperience(entity, userId);

            var filter = Builders<Profile>.Filter.And(
                Builders<Profile>.Filter.Eq(p => p.UserId, userId),
                Builders<Profile>.Filter.ElemMatch(p => p.Experiences, e => e.Id == entityId)
            );

            var update = Builders<Profile>.Update.Set("Experiences.$", entity);
            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteFromProfileAsync(string userId, string entityId)
        {
            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Profile>.Update.PullFilter(p => p.Experiences, e => e.Id == entityId);

            var result = await _profileCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // Método auxiliar para carregar skills completas
        private async Task<Experience> LoadSkillsForExperience(Experience experience, string userId)
        {
            if (experience.SkillIds?.Any() == true)
            {
                var allUserSkills = await _skillRepository.GetByUserIdAsync(userId);
                experience.Skills = allUserSkills
                    .Where(s => experience.SkillIds.Contains(s.Id))
                    .ToList();
            }
            else
            {
                experience.Skills = new List<Skill>();
            }

            return experience;
        }
    }
}