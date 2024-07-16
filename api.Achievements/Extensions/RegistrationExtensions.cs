using api.Achievements.Interfaces;
using api.Achievements.Models;
using api.Achievements.Queries;
using api.Achievements.Services;
using api.Achievements.TypeExtensions;

namespace api.Achievements.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiAchievements(this IServiceCollection services, IConfiguration config, string sectionPath = "data")
    {
        services
            .AddOptions<AchievementsDataConfig>()
            .Bind(config.GetSection(sectionPath));

        services.AddScoped<IDataRepository<IAchievement>, AchievementDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiAchievements(this IRequestExecutorBuilder builder) =>
        builder
            .AddTypeExtension(typeof(AchievementQueries))
            .AddTypeExtension<AchievementTypeExtension>();
}
