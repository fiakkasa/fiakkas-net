using api.Languages.Interfaces;
using api.Languages.Models;
using api.Languages.Queries;
using api.Languages.Services;

namespace api.Languages.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiLanguages(this IServiceCollection services, IConfiguration config, string sectionPath = "data")
    {
        services
            .AddOptions<LanguagesDataConfig>()
            .Bind(config.GetSection(sectionPath));

        services.AddScoped<IDataRepository<ILanguage>, LanguageDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiLanguages(this IRequestExecutorBuilder builder) =>
        builder
            .AddTypeExtension<LanguageQueries>();
}
