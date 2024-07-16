using api.TextItems.Interfaces;
using api.TextItems.Models;
using api.TextItems.Queries;
using api.TextItems.Services;

namespace api.TextItems.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiTextItems(this IServiceCollection services, IConfiguration config, string sectionPath = "data")
    {
        services
            .AddOptions<TextItemsDataConfig>()
            .Bind(config.GetSection(sectionPath));

        services.AddScoped<IDataRepository<ITextItem>, TextItemDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiTextItems(this IRequestExecutorBuilder builder) =>
        builder
            .AddTypeExtension(typeof(TextItemQueries));
}
