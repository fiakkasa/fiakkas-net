namespace app.Shared.Logging.Tests.Extensions;

public class LoggingRegistrationExtensionsTests
{
    [Fact]
    public void AddApiLoggingProvider_Adds_Logging_Provider_To_Host()
    {
        var mockHost = Substitute.For<IHostBuilder>();

        var result = mockHost.AddAppLoggingProvider();

        Assert.Same(mockHost, result);
        mockHost.Received(1).ConfigureServices(Arg.Any<Action<HostBuilderContext, IServiceCollection>>());
    }

    [Fact]
    public void UseApiLoggingProvider_Enables_Logging_Provider_In_Pipeline()
    {
        var mockApp = Substitute.For<IApplicationBuilder>();

        mockApp.UseAppLoggingProvider();

        Assert.Same(mockApp, mockApp);
        mockApp.ApplicationServices.Received(1).GetService<IOptions<RequestLoggingOptions>>();
        mockApp.Received(1).Use(Arg.Any<Func<RequestDelegate, RequestDelegate>>());
    }
}
