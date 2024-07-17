using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace api.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static OptionsBuilder<T> AddBoundOptions<T>(
        this IServiceCollection services,
        IConfiguration config,
        string? sectionPath = default
    ) where T : class, new()
    {
        var resolvedSectionPath = sectionPath?.Trim() switch
        {
            { Length: > 0 } path => path,
            _ => typeof(T).Name
        };

        return services
            .AddOptions<T>()
            .Bind(config.GetSection(resolvedSectionPath));
    }
}
