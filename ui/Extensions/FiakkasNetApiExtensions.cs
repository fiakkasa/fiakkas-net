using Microsoft.Extensions.Options;
using ui.Models;

namespace ui.Extensions;

public static class FiakkasNetApiExtensions
{
    public static IServiceCollection AddFiakkasNetApiClient(this IServiceCollection services)
    {
        services
            .AddOptions<FiakkasNetApiConfig>()
            .BindConfiguration(nameof(FiakkasNetApiConfig))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddFiakkasNetApi()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var apiConfig = serviceProvider.GetRequiredService<IOptionsMonitor<FiakkasNetApiConfig>>().CurrentValue;
                client.BaseAddress = apiConfig.BaseUrl;
            });

        return services;
    }
}
