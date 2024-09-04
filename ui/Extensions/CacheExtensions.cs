namespace ui.Extensions;

public static class CacheExtensions
{
    public static IServiceCollection AddUiCache(this IServiceCollection services) =>
        services.AddLazyCache();
}
