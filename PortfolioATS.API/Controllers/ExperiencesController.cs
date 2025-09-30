using Microsoft.AspNetCore.Mvc;
using PortfolioATS.Core.DTOs;
using PortfolioATS.Core.Interfaces;

namespace PortfolioATS.API.Controllers
{
    public class ExperiencesController : BaseEmbeddedController
    {
        private readonly IExperienceRepository _experienceRepository;
        private readonly ISkillRepository _skillRepository;

        public ExperiencesController(IExperienceRepository experienceRepository, ISkillRepository skillRepository)
        {
            _experienceRepository = experienceRepository;
            _skillRepository = skillRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetExperiences()
        {
            try
            {
                var userId = GetUserId();
                var experiences = await _experienceRepository.GetByUserIdAsync(userId);

                var experienceDtos = experiences.Select(e => new ExperienceDto
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
                });

                return Ok(experienceDtos);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<ExperienceDto>>(ex);
            }
        }

        [HttpGet("current")]
        public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetCurrentExperiences()
        {
            try
            {
                var userId = GetUserId();
                var experiences = await _experienceRepository.GetCurrentExperiencesAsync(userId);

                var experienceDtos = experiences.Select(e => new ExperienceDto
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
                });

                return Ok(experienceDtos);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<ExperienceDto>>(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ExperienceDto>> AddExperience([FromBody] CreateExperienceRequest request)
        {
            try
            {
                var userId = GetUserId();
                var experience = new Core.Entities.Experience
                {
                    Company = request.Company,
                    Position = request.Position,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsCurrent = request.IsCurrent,
                    Description = request.Description,
                    EmploymentType = request.EmploymentType,
                    SkillIds = request.SkillIds
                };

                // Buscar skills relacionadas para incluir no response
                var allSkills = await _skillRepository.GetByUserIdAsync(userId);
                experience.Skills = allSkills.Where(s => request.SkillIds.Contains(s.Id)).ToList();

                var result = await _experienceRepository.AddToProfileAsync(userId, experience);

                var experienceDto = new ExperienceDto
                {
                    Id = result.Id,
                    Company = result.Company,
                    Position = result.Position,
                    StartDate = result.StartDate,
                    EndDate = result.EndDate,
                    IsCurrent = result.IsCurrent,
                    Description = result.Description,
                    EmploymentType = result.EmploymentType,
                    SkillIds = result.SkillIds,
                    Skills = result.Skills.Select(s => new SkillDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Category = s.Category,
                        Level = s.Level,
                        YearsOfExperience = s.YearsOfExperience
                    }).ToList()
                };

                return Ok(experienceDto);
            }
            catch (Exception ex)
            {
                return HandleException<ExperienceDto>(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateExperience(string id, [FromBody] CreateExperienceRequest request)
        {
            try
            {
                var userId = GetUserId();
                var experience = new Core.Entities.Experience
                {
                    Id = id,
                    Company = request.Company,
                    Position = request.Position,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsCurrent = request.IsCurrent,
                    Description = request.Description,
                    EmploymentType = request.EmploymentType,
                    SkillIds = request.SkillIds
                };

                var success = await _experienceRepository.UpdateInProfileAsync(userId, id, experience);
                if (!success)
                {
                    return NotFound(new { message = "Experiência não encontrada." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleExceptionWithNoContent(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExperience(string id)
        {
            try
            {
                var userId = GetUserId();
                var success = await _experienceRepository.DeleteFromProfileAsync(userId, id);
                if (!success)
                {
                    return NotFound(new { message = "Experiência não encontrada." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleExceptionWithNoContent(ex);
            }
        }
    }
}