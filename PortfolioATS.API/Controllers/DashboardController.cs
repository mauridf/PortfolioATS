using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioATS.Core.DTOs;
using PortfolioATS.Core.Interfaces;
using System.Security.Claims;

namespace PortfolioATS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboard()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var dashboard = await _dashboardService.GetDashboardDataAsync(userId);
                return Ok(dashboard);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor." });
            }
        }

        [HttpGet("completion")]
        public async Task<ActionResult<int>> GetProfileCompletion()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var completion = await _dashboardService.CalculateProfileCompletionAsync(userId);
                return Ok(completion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor." });
            }
        }

        [HttpGet("ats-score")]
        public async Task<ActionResult<AtsScoreDto>> GetAtsScore()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var atsScore = await _dashboardService.CalculateAtsScoreAsync(userId);
                return Ok(atsScore);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor." });
            }
        }
    }
}