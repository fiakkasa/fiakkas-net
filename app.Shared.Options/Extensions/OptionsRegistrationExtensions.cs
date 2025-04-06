namespace app.Shared.Options.Extensions;

public static class OptionsRegistrationExtensions
{
    public static OptionsBuilder<TOptions> AddBoundOptions<TOptions>(
        this IServiceCollection services,
        string? sectionPath = default
    ) where TOptions : class, new()
    {
        var resolvedSectionPath = sectionPath?.Trim() switch
        {
            { Length: > 0 } path => path,
            _ => typeof(TOptions).Name
        };

        return services
            .AddOptions<TOptions>()
            .BindConfiguration(resolvedSectionPath);
    }

    public static OptionsBuilder<TOptions> AddValidatedOptions<TOptions>(
        this IServiceCollection services,
        string? sectionPath = default
    ) where TOptions : class, new() =>
        services
            .AddBoundOptions<TOptions>(sectionPath)
            .ValidateDataAnnotations()
            .ValidateOnStart();
}
