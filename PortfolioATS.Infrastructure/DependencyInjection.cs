using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PortfolioATS.Core.Interfaces;
using PortfolioATS.Infrastructure.Data;
using PortfolioATS.Infrastructure.Repositories;

namespace PortfolioATS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuração do MongoDB - FORMA CORRETA
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDBSettings"));

            // Registrar MongoDBSettings como singleton para acesso direto se necessário
            services.AddSingleton<MongoDBSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

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

            return services;
        }
    }
}