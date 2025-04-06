namespace app.Shared.Logging.Extensions;

public static class LoggingRegistrationExtensions
{
    public static IHostBuilder AddAppLoggingProvider(this IHostBuilder hostBuilder) =>
        hostBuilder.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)
        );

    public static IApplicationBuilder UseAppLoggingProvider(this IApplicationBuilder app) =>
        app.UseSerilogRequestLogging();
}
