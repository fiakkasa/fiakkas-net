using ui.Models;

namespace ui.Extensions;

public static class UiExtensions
{
    public static IServiceCollection AddUiConfig(this IServiceCollection services)
    {
        services.AddValidatedOptions<UiConfig>();

        return services;
    }
}
