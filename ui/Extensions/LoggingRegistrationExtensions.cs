using Serilog;

namespace ui.Extensions;

public static class LoggingRegistrationExtensions
{
    public static IHostBuilder AddUiLoggingProvider(this IHostBuilder hostBuilder) =>
        hostBuilder.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)
        );

    public static IApplicationBuilder UseUiLoggingProvider(this IApplicationBuilder app) =>
        app.UseSerilogRequestLogging();
}
