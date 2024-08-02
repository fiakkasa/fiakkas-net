using Microsoft.AspNetCore.Http;

namespace ui.Extensions.Tests;

public class LoggingRegistrationExtensionsTests
{
    [Fact]
    public void AddUiLoggingProvider_Adds_Logging_Provider_To_Host()
    {
        var mockHost = Substitute.For<IHostBuilder>();

        mockHost.AddUiLoggingProvider();

        Assert.Single(
            mockHost
                .ReceivedCalls()
                .Select(x => x.GetOriginalArguments())
                .SelectMany(x => x)
                .OfType<Action<HostBuilderContext, IServiceCollection>>()
        );
    }

    [Fact]
    public void UseUiLoggingProvider_Enables_Logging_Provider_In_Pipeline()
    {
        var mockApp = Substitute.For<IApplicationBuilder>();

        mockApp.UseUiLoggingProvider();

        Assert.Single(
            mockApp
                .ReceivedCalls()
                .Select(x => x.GetOriginalArguments())
                .SelectMany(x => x)
                .OfType<Func<RequestDelegate, RequestDelegate>>()
        );
    }
}
