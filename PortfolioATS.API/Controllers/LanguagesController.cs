using Microsoft.AspNetCore.Mvc;
using PortfolioATS.Core.DTOs;
using PortfolioATS.Core.Interfaces;

namespace PortfolioATS.API.Controllers
{
    public class LanguagesController : BaseEmbeddedController
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguagesController(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LanguageDto>>> GetLanguages()
        {
            try
            {
                var userId = GetUserId();
                var languages = await _languageRepository.GetByUserIdAsync(userId);

                var languageDtos = languages.Select(l => new LanguageDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    Proficiency = l.Proficiency
                });

                return Ok(languageDtos);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<LanguageDto>>(ex);
            }
        }

        [HttpGet("proficiency/{proficiency}")]
        public async Task<ActionResult<IEnumerable<LanguageDto>>> GetLanguagesByProficiency(string proficiency)
        {
            try
            {
                var userId = GetUserId();
                var languages = await _languageRepository.GetByUserIdAsync(userId);
                var filteredLanguages = languages.Where(l => l.Proficiency.Equals(proficiency, StringComparison.OrdinalIgnoreCase));

                var languageDtos = filteredLanguages.Select(l => new LanguageDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    Proficiency = l.Proficiency
                });

                return Ok(languageDtos);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<LanguageDto>>(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<LanguageDto>> AddLanguage([FromBody] CreateLanguageRequest request)
        {
            try
            {
                var userId = GetUserId();
                var language = new Core.Entities.Language
                {
                    UserId = userId,
                    Name = request.Name,
                    Proficiency = request.Proficiency
                };

                var result = await _languageRepository.AddToProfileAsync(userId, language);

                var languageDto = new LanguageDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    Proficiency = result.Proficiency
                };

                return Ok(languageDto);
            }
            catch (Exception ex)
            {
                return HandleException<LanguageDto>(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLanguage(string id, [FromBody] CreateLanguageRequest request)
        {
            try
            {
                var userId = GetUserId();

                // Buscar o idioma existente para validar ownership
                var existingLanguages = await _languageRepository.GetByUserIdAsync(userId);
                var existingLanguage = existingLanguages.FirstOrDefault(l => l.Id == id);

                if (existingLanguage == null)
                {
                    return NotFound(new { message = "Idioma não encontrado." });
                }

                // Validar se o usuário é o dono do idioma
                ValidateUserOwnership(existingLanguage.UserId);

                var language = new Core.Entities.Language
                {
                    Id = id,
                    UserId = userId,
                    Name = request.Name,
                    Proficiency = request.Proficiency
                };

                var success = await _languageRepository.UpdateInProfileAsync(userId, id, language);
                if (!success)
                {
                    return NotFound(new { message = "Idioma não encontrado." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleExceptionWithNoContent(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLanguage(string id)
        {
            try
            {
                var userId = GetUserId();

                // Buscar o idioma existente para validar ownership
                var existingLanguages = await _languageRepository.GetByUserIdAsync(userId);
                var existingLanguage = existingLanguages.FirstOrDefault(l => l.Id == id);

                if (existingLanguage == null)
                {
                    return NotFound(new { message = "Idioma não encontrado." });
                }

                // Validar se o usuário é o dono do idioma
                ValidateUserOwnership(existingLanguage.UserId);

                var success = await _languageRepository.DeleteFromProfileAsync(userId, id);
                if (!success)
                {
                    return NotFound(new { message = "Idioma não encontrado." });
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