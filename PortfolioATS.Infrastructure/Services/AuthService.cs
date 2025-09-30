using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PortfolioATS.Core.DTOs;
using PortfolioATS.Core.Entities;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Core.Models;

namespace PortfolioATS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            IUserRepository userRepository,
            IProfileRepository profileRepository,
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // Validar se as senhas coincidem
            if (request.Password != request.ConfirmPassword)
            {
                throw new ArgumentException("As senhas não coincidem.");
            }

            // Verificar se o email já existe
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                throw new ArgumentException("Este email já está em uso.");
            }

            // Criar usuário
            var user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "User",
                IsActive = true
            };

            await _userRepository.AddAsync(user);

            // Criar perfil associado
            var profile = new Profile
            {
                UserId = user.Id,
                FullName = request.FullName,
                Email = request.Email
            };

            await _profileRepository.AddAsync(profile);

            // Atualizar user com profileId
            user.ProfileId = profile.Id;
            await _userRepository.UpdateAsync(user);

            // Gerar token
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = request.FullName,
                Role = user.Role
            };

            var token = GenerateJwtToken(userDto);

            return new AuthResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                User = userDto
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // Buscar usuário por email
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }

            // Verificar senha
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }

            // Buscar perfil para obter o nome completo
            var profile = await _profileRepository.GetByUserIdAsync(user.Id);

            // Atualizar último login
            user.LastLogin = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            // Gerar token
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = profile?.FullName ?? string.Empty,
                Role = user.Role
            };

            var token = GenerateJwtToken(userDto);

            return new AuthResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                User = userDto
            };
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("Usuário não encontrado.");
            }

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Senha atual incorreta.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public string GenerateJwtToken(UserDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}