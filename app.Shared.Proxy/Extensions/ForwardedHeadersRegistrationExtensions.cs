using app.Shared.Proxy.Mappers;
using app.Shared.Proxy.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace app.Shared.Proxy.Extensions;

public static class ForwardedHeadersRegistrationExtensions
{
    /// <summary>
    ///     Registers the forwarded headers middleware with the application.
    ///     It's important to note that an instance of ForwardedHeadersConfig must be registered in the DI container using the
    ///     Options pattern.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAppForwardedHeaders(this IApplicationBuilder app) =>
        app.ApplicationServices.GetRequiredService<IOptionsMonitor<ForwardedHeadersConfig>>().CurrentValue switch
        {
            { Enable: true } config => app.UseForwardedHeaders(config.ToModel()),
            _ => app
        };
}
