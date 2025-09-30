using PortfolioATS.Core.DTOs;

namespace PortfolioATS.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        string GenerateJwtToken(UserDto user);
    }
}