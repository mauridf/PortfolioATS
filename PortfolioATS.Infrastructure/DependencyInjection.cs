using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            // Configuração
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDBSettings"));
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

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
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IDashboardService, DashboardService>();

            // Inicializar índices do MongoDB
            services.AddHostedService<MongoDBIndexService>();

            return services;
        }
    }

    public class MongoDBIndexService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public MongoDBIndexService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MongoDBContext>();
            await context.EnsureIndexesCreatedAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}