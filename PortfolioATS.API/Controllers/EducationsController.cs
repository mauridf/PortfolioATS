using Microsoft.AspNetCore.Mvc;
using PortfolioATS.Core.DTOs;
using PortfolioATS.Core.Interfaces;

namespace PortfolioATS.API.Controllers
{
    public class EducationsController : BaseEmbeddedController
    {
        private readonly IEducationRepository _educationRepository;

        public EducationsController(IEducationRepository educationRepository)
        {
            _educationRepository = educationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EducationDto>>> GetEducations()
        {
            try
            {
                var userId = GetUserId();
                var educations = await _educationRepository.GetByUserIdAsync(userId);

                var educationDtos = educations.Select(e => new EducationDto
                {
                    Id = e.Id,
                    Institution = e.Institution,
                    Degree = e.Degree,
                    FieldOfStudy = e.FieldOfStudy,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    IsCompleted = e.IsCompleted,
                    Description = e.Description
                });

                return Ok(educationDtos);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<EducationDto>>(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<EducationDto>> AddEducation([FromBody] CreateEducationRequest request)
        {
            try
            {
                var userId = GetUserId();
                var education = new Core.Entities.Education
                {
                    Institution = request.Institution,
                    Degree = request.Degree,
                    FieldOfStudy = request.FieldOfStudy,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsCompleted = request.IsCompleted,
                    Description = request.Description
                };

                var result = await _educationRepository.AddToProfileAsync(userId, education);

                var educationDto = new EducationDto
                {
                    Id = result.Id,
                    Institution = result.Institution,
                    Degree = result.Degree,
                    FieldOfStudy = result.FieldOfStudy,
                    StartDate = result.StartDate,
                    EndDate = result.EndDate,
                    IsCompleted = result.IsCompleted,
                    Description = result.Description
                };

                return Ok(educationDto);
            }
            catch (Exception ex)
            {
                return HandleException<EducationDto>(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEducation(string id, [FromBody] CreateEducationRequest request)
        {
            try
            {
                var userId = GetUserId();
                var education = new Core.Entities.Education
                {
                    Id = id,
                    Institution = request.Institution,
                    Degree = request.Degree,
                    FieldOfStudy = request.FieldOfStudy,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsCompleted = request.IsCompleted,
                    Description = request.Description
                };

                var success = await _educationRepository.UpdateInProfileAsync(userId, id, education);
                if (!success)
                {
                    return NotFound(new { message = "Formação não encontrada." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleExceptionWithNoContent(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEducation(string id)
        {
            try
            {
                var userId = GetUserId();
                var success = await _educationRepository.DeleteFromProfileAsync(userId, id);
                if (!success)
                {
                    return NotFound(new { message = "Formação não encontrada." });
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