using api.Achievements.Interfaces;
using api.Achievements.Models;
using api.Achievements.Services;

namespace api.Achievements.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiAchievements(
        this IServiceCollection services,
        IConfiguration config,
        string sectionPath = "data"
    )
    {
        services.AddBoundOptions<AchievementsDataConfig>(config, sectionPath);

        services.AddScoped<IDataRepository<IAchievement>, AchievementDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiAchievements(this IRequestExecutorBuilder builder) =>
        builder.AddAchievementsGraph();
}
