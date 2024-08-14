namespace ui.Extensions;

public static class OptionsExtensions
{
    public static OptionsBuilder<TOptions> AddValidatedOptions<TOptions>(
        this IServiceCollection services,
        string? sectionKey = default
    ) where TOptions : class
        => services
            .AddOptions<TOptions>()
            .BindConfiguration(sectionKey ?? typeof(TOptions).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();
}
