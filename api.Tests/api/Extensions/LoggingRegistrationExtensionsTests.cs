using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace api.Extensions.Tests;

public class LoggingRegistrationExtensionsTests
{
    [Fact]
    public void AddApiLoggingProvider_Adds_Logging_Provider_To_Host()
    {
        var mockHost = Substitute.For<IHostBuilder>();

        mockHost.AddApiLoggingProvider();

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

        mockApp.UseApiLoggingProvider();

        Assert.Single(
            mockApp
                .ReceivedCalls()
                .Select(x => x.GetOriginalArguments())
                .SelectMany(x => x)
                .OfType<Func<RequestDelegate, RequestDelegate>>()
        );
    }
}
