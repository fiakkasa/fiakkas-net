using ui.Models;

namespace ui.Extensions;

public static class UiExtensions
{
    public static IServiceCollection AddUiConfig(this IServiceCollection services)
    {
        services
            .AddOptions<UiConfig>()
            .BindConfiguration(nameof(UiConfig))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
