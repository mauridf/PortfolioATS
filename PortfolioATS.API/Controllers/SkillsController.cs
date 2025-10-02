using Microsoft.AspNetCore.Mvc;
using PortfolioATS.Core.DTOs;
using PortfolioATS.Core.Interfaces;

namespace PortfolioATS.API.Controllers
{
    public class SkillsController : BaseEmbeddedController
    {
        private readonly ISkillRepository _skillRepository;

        public SkillsController(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetSkills()
        {
            try
            {
                var userId = GetUserId();
                var skills = await _skillRepository.GetByUserIdAsync(userId);

                var skillDtos = skills.Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category,
                    Level = s.Level,
                    YearsOfExperience = s.YearsOfExperience
                });

                return Ok(skillDtos);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<SkillDto>>(ex);
            }
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetSkillsByCategory(string category)
        {
            try
            {
                var userId = GetUserId();
                var skills = await _skillRepository.GetByUserIdAndCategoryAsync(userId, category);

                var skillDtos = skills.Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category,
                    Level = s.Level,
                    YearsOfExperience = s.YearsOfExperience
                });

                return Ok(skillDtos);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<SkillDto>>(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<SkillDto>> AddSkill([FromBody] CreateSkillRequest request)
        {
            try
            {
                var userId = GetUserId();
                var skill = new Core.Entities.Skill
                {
                    UserId = userId,
                    Name = request.Name,
                    Category = request.Category,
                    Level = request.Level,
                    YearsOfExperience = request.YearsOfExperience
                };

                var result = await _skillRepository.AddToProfileAsync(userId, skill);

                var skillDto = new SkillDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    Category = result.Category,
                    Level = result.Level,
                    YearsOfExperience = result.YearsOfExperience
                };

                return Ok(skillDto);
            }
            catch (Exception ex)
            {
                return HandleException<SkillDto>(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSkill(string id, [FromBody] CreateSkillRequest request)
        {
            try
            {
                var userId = GetUserId();

                // Buscar a skill existente para validar ownership
                var existingSkills = await _skillRepository.GetByUserIdAsync(userId);
                var existingSkill = existingSkills.FirstOrDefault(s => s.Id == id);

                if (existingSkill == null)
                {
                    return NotFound(new { message = "Skill não encontrada." });
                }

                // Validar se o usuário é o dono da skill
                ValidateUserOwnership(existingSkill.UserId);

                var skill = new Core.Entities.Skill
                {
                    Id = id,
                    UserId = userId,
                    Name = request.Name,
                    Category = request.Category,
                    Level = request.Level,
                    YearsOfExperience = request.YearsOfExperience
                };

                var success = await _skillRepository.UpdateInProfileAsync(userId, id, skill);
                if (!success)
                {
                    return NotFound(new { message = "Skill não encontrada." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleExceptionWithNoContent(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSkill(string id)
        {
            try
            {
                var userId = GetUserId();

                // Buscar a skill existente para validar ownership
                var existingSkills = await _skillRepository.GetByUserIdAsync(userId);
                var existingSkill = existingSkills.FirstOrDefault(s => s.Id == id);

                if (existingSkill == null)
                {
                    return NotFound(new { message = "Skill não encontrada." });
                }

                // Validar se o usuário é o dono da skill
                ValidateUserOwnership(existingSkill.UserId);

                var success = await _skillRepository.DeleteFromProfileAsync(userId, id);
                if (!success)
                {
                    return NotFound(new { message = "Skill não encontrada." });
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