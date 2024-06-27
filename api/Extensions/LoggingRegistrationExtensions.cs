using Serilog;

namespace api.Extensions;

public static class LoggingRegistrationExtensions
{
    public static IHostBuilder AddApiLoggingProvider(this IHostBuilder hostBuilder) =>
        hostBuilder.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)
        );

    public static IApplicationBuilder UseApiLoggingProvider(this IApplicationBuilder app) =>
        app.UseSerilogRequestLogging();
}
