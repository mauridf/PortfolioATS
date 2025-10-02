using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PortfolioATS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public abstract class BaseEmbeddedController : ControllerBase
    {
        protected string GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            }
            return userId;
        }

        // Validar se o usuário tem acesso à entidade
        protected void ValidateUserOwnership(string entityUserId)
        {
            var currentUserId = GetUserId();
            if (entityUserId != currentUserId)
            {
                throw new UnauthorizedAccessException("Acesso não autorizado a este recurso.");
            }
        }

        protected ActionResult<T> HandleException<T>(Exception ex)
        {
            if (ex is UnauthorizedAccessException)
                return Unauthorized(new { message = ex.Message });
            else if (ex is ArgumentException)
                return BadRequest(new { message = ex.Message });
            else if (ex is KeyNotFoundException)
                return NotFound(new { message = ex.Message });
            else
                return StatusCode(500, new { message = "Erro interno do servidor." });
        }

        protected ActionResult HandleExceptionWithNoContent(Exception ex)
        {
            if (ex is UnauthorizedAccessException)
                return Unauthorized(new { message = ex.Message });
            else if (ex is ArgumentException)
                return BadRequest(new { message = ex.Message });
            else if (ex is KeyNotFoundException)
                return NotFound(new { message = ex.Message });
            else
                return StatusCode(500, new { message = "Erro interno do servidor." });
        }
    }
}