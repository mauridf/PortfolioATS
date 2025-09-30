using PortfolioATS.Core.DTOs;

namespace PortfolioATS.Core.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileDto> GetProfileByUserIdAsync(string userId);
        Task<ProfileDto> UpdateProfileAsync(string userId, UpdateProfileRequest request);
        Task<bool> ProfileExistsAsync(string userId);
    }
}