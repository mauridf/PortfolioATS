using Microsoft.AspNetCore.Mvc;
using PortfolioATS.Core.DTOs;
using PortfolioATS.Core.Interfaces;

namespace PortfolioATS.API.Controllers
{
    public class SocialLinksController : BaseEmbeddedController
    {
        private readonly ISocialLinkRepository _socialLinkRepository;

        public SocialLinksController(ISocialLinkRepository socialLinkRepository)
        {
            _socialLinkRepository = socialLinkRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SocialLinkDto>>> GetSocialLinks()
        {
            try
            {
                var userId = GetUserId();
                var socialLinks = await _socialLinkRepository.GetByUserIdAsync(userId);

                var socialLinkDtos = socialLinks.Select(sl => new SocialLinkDto
                {
                    Id = sl.Id,
                    Platform = sl.Platform,
                    Url = sl.Url,
                    Username = sl.Username
                });

                return Ok(socialLinkDtos);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<SocialLinkDto>>(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<SocialLinkDto>> AddSocialLink([FromBody] CreateSocialLinkRequest request)
        {
            try
            {
                var userId = GetUserId();
                var socialLink = new Core.Entities.SocialLink
                {
                    UserId = userId,
                    Platform = request.Platform,
                    Url = request.Url,
                    Username = request.Username
                };

                var result = await _socialLinkRepository.AddToProfileAsync(userId, socialLink);

                var socialLinkDto = new SocialLinkDto
                {
                    Id = result.Id,
                    Platform = result.Platform,
                    Url = result.Url,
                    Username = result.Username
                };

                return Ok(socialLinkDto);
            }
            catch (Exception ex)
            {
                return HandleException<SocialLinkDto>(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSocialLink(string id, [FromBody] CreateSocialLinkRequest request)
        {
            try
            {
                var userId = GetUserId();

                // Buscar o social link existente para validar ownership
                var existingSocialLinks = await _socialLinkRepository.GetByUserIdAsync(userId);
                var existingSocialLink = existingSocialLinks.FirstOrDefault(sl => sl.Id == id);

                if (existingSocialLink == null)
                {
                    return NotFound(new { message = "Social link não encontrado." });
                }

                // Validar se o usuário é o dono do social link
                ValidateUserOwnership(existingSocialLink.UserId);

                var socialLink = new Core.Entities.SocialLink
                {
                    Id = id,
                    UserId = userId,
                    Platform = request.Platform,
                    Url = request.Url,
                    Username = request.Username
                };

                var success = await _socialLinkRepository.UpdateInProfileAsync(userId, id, socialLink);
                if (!success)
                {
                    return NotFound(new { message = "Social link não encontrado." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleExceptionWithNoContent(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSocialLink(string id)
        {
            try
            {
                var userId = GetUserId();

                // Buscar o social link existente para validar ownership
                var existingSocialLinks = await _socialLinkRepository.GetByUserIdAsync(userId);
                var existingSocialLink = existingSocialLinks.FirstOrDefault(sl => sl.Id == id);

                if (existingSocialLink == null)
                {
                    return NotFound(new { message = "Social link não encontrado." });
                }

                // Validar se o usuário é o dono do social link
                ValidateUserOwnership(existingSocialLink.UserId);

                var success = await _socialLinkRepository.DeleteFromProfileAsync(userId, id);
                if (!success)
                {
                    return NotFound(new { message = "Social link não encontrado." });
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