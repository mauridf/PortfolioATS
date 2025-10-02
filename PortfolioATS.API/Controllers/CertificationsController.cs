using Microsoft.AspNetCore.Mvc;
using PortfolioATS.Core.DTOs;
using PortfolioATS.Core.Interfaces;

namespace PortfolioATS.API.Controllers
{
    public class CertificationsController : BaseEmbeddedController
    {
        private readonly ICertificationRepository _certificationRepository;

        public CertificationsController(ICertificationRepository certificationRepository)
        {
            _certificationRepository = certificationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CertificationDto>>> GetCertifications()
        {
            try
            {
                var userId = GetUserId();
                var certifications = await _certificationRepository.GetByUserIdAsync(userId);

                var certificationDtos = certifications.Select(c => new CertificationDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IssuingOrganization = c.IssuingOrganization,
                    IssueDate = c.IssueDate,
                    ExpirationDate = c.ExpirationDate,
                    CredentialId = c.CredentialId,
                    CredentialUrl = c.CredentialUrl
                });

                return Ok(certificationDtos);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<CertificationDto>>(ex);
            }
        }

        [HttpGet("expired")]
        public async Task<ActionResult<IEnumerable<CertificationDto>>> GetExpiredCertifications()
        {
            try
            {
                var userId = GetUserId();
                var certifications = await _certificationRepository.GetExpiredCertificationsAsync(userId);

                var certificationDtos = certifications.Select(c => new CertificationDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IssuingOrganization = c.IssuingOrganization,
                    IssueDate = c.IssueDate,
                    ExpirationDate = c.ExpirationDate,
                    CredentialId = c.CredentialId,
                    CredentialUrl = c.CredentialUrl
                });

                return Ok(certificationDtos);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<CertificationDto>>(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CertificationDto>> AddCertification([FromBody] CreateCertificationRequest request)
        {
            try
            {
                var userId = GetUserId();
                var certification = new Core.Entities.Certification
                {
                    UserId = userId,
                    Name = request.Name,
                    IssuingOrganization = request.IssuingOrganization,
                    IssueDate = request.IssueDate,
                    ExpirationDate = request.ExpirationDate,
                    CredentialId = request.CredentialId,
                    CredentialUrl = request.CredentialUrl
                };

                var result = await _certificationRepository.AddToProfileAsync(userId, certification);

                var certificationDto = new CertificationDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    IssuingOrganization = result.IssuingOrganization,
                    IssueDate = result.IssueDate,
                    ExpirationDate = result.ExpirationDate,
                    CredentialId = result.CredentialId,
                    CredentialUrl = result.CredentialUrl
                };

                return Ok(certificationDto);
            }
            catch (Exception ex)
            {
                return HandleException<CertificationDto>(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCertification(string id, [FromBody] CreateCertificationRequest request)
        {
            try
            {
                var userId = GetUserId();

                // Buscar a certificação existente para validar ownership
                var existingCertifications = await _certificationRepository.GetByUserIdAsync(userId);
                var existingCertification = existingCertifications.FirstOrDefault(c => c.Id == id);

                if (existingCertification == null)
                {
                    return NotFound(new { message = "Certificação não encontrada." });
                }

                // Validar se o usuário é o dono da certificação
                ValidateUserOwnership(existingCertification.UserId);

                var certification = new Core.Entities.Certification
                {
                    Id = id,
                    UserId = userId,
                    Name = request.Name,
                    IssuingOrganization = request.IssuingOrganization,
                    IssueDate = request.IssueDate,
                    ExpirationDate = request.ExpirationDate,
                    CredentialId = request.CredentialId,
                    CredentialUrl = request.CredentialUrl
                };

                var success = await _certificationRepository.UpdateInProfileAsync(userId, id, certification);
                if (!success)
                {
                    return NotFound(new { message = "Certificação não encontrada." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleExceptionWithNoContent(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCertification(string id)
        {
            try
            {
                var userId = GetUserId();

                // Buscar a certificação existente para validar ownership
                var existingCertifications = await _certificationRepository.GetByUserIdAsync(userId);
                var existingCertification = existingCertifications.FirstOrDefault(c => c.Id == id);

                if (existingCertification == null)
                {
                    return NotFound(new { message = "Certificação não encontrada." });
                }

                // Validar se o usuário é o dono da certificação
                ValidateUserOwnership(existingCertification.UserId);

                var success = await _certificationRepository.DeleteFromProfileAsync(userId, id);
                if (!success)
                {
                    return NotFound(new { message = "Certificação não encontrada." });
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