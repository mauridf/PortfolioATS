using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Core.Models;
using PortfolioATS.Infrastructure.Data;
using PortfolioATS.Infrastructure.Repositories;
using PortfolioATS.Infrastructure.Services;

namespace PortfolioATS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuração do MongoDB
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDBSettings"));
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            // Registrar configurações como singleton para acesso direto
            services.AddSingleton<MongoDBSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);
            services.AddSingleton<JwtSettings>(sp =>
                sp.GetRequiredService<IOptions<JwtSettings>>().Value);

            // MongoDB Context
            services.AddSingleton<MongoDBContext>();

            // Repositórios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IExperienceRepository, ExperienceRepository>();
            services.AddScoped<IEducationRepository, EducationRepository>();
            services.AddScoped<ICertificationRepository, CertificationRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ISocialLinkRepository, SocialLinkRepository>();

            // Serviços
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}