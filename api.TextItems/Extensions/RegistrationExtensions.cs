using api.TextItems.Interfaces;
using api.TextItems.Models;
using api.TextItems.Services;

namespace api.TextItems.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddApiTextItems(
        this IServiceCollection services,
        string sectionPath = "data"
    )
    {
        services.AddValidatedOptions<TextItemsDataConfig>(sectionPath);

        services.AddScoped<IDataRepository<ITextItem>, TextItemDataRepository>();

        return services;
    }

    public static IRequestExecutorBuilder AddApiTextItems(this IRequestExecutorBuilder builder) =>
        builder.AddTextItemsGraph();
}
