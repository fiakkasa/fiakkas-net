namespace app.Shared.Logging.Tests.Extensions;

public class LoggingRegistrationExtensionsTests
{
    [Fact]
    public void AddApiLoggingProvider_Adds_Logging_Provider_To_Host()
    {
        var mockHost = Substitute.For<IHostBuilder>();

        mockHost.AddAppLoggingProvider();

        Assert.Single(
            mockHost
                .ReceivedCalls()
                .Select(x => x.GetOriginalArguments())
                .SelectMany(x => x)
                .OfType<Action<HostBuilderContext, IServiceCollection>>()
        );
    }

    [Fact]
    public void UseApiLoggingProvider_Enables_Logging_Provider_In_Pipeline()
    {
        var mockApp = Substitute.For<IApplicationBuilder>();

        mockApp.UseAppLoggingProvider();

        Assert.Single(
            mockApp
                .ReceivedCalls()
                .Select(x => x.GetOriginalArguments())
                .SelectMany(x => x)
                .OfType<Func<RequestDelegate, RequestDelegate>>()
        );
    }
}
