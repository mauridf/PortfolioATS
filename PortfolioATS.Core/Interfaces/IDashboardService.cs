using PortfolioATS.Core.DTOs;

namespace PortfolioATS.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync(string userId);
        Task<int> CalculateProfileCompletionAsync(string userId);
        Task<AtsScoreDto> CalculateAtsScoreAsync(string userId);
    }
}