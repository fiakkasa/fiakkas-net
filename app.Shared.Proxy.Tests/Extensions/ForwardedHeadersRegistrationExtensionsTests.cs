namespace app.Shared.Proxy.Tests.Extensions;

public class ForwardedHeadersRegistrationExtensionsTests
{
    [Fact]
    public void UseAppForwardedHeaders_When_UseForwardedHeaders_Disabled_App_Does_Not_Register_Forwarded_Headers()
    {
        var mockOptions = Substitute.For<IOptionsMonitor<ForwardedHeadersConfig>>();
        mockOptions.CurrentValue.Returns(new ForwardedHeadersConfig
        {
            Enable = false
        });
        var services = new ServiceCollection();
        services.AddSingleton(mockOptions);
        var mockApp = Substitute.For<IApplicationBuilder>();
        mockApp.ApplicationServices.Returns(services.BuildServiceProvider());

        mockApp.UseAppForwardedHeaders();

        Assert.Single(mockOptions.ReceivedCalls());
        mockApp
            .DidNotReceive()
            .Use(
                Arg.Is<Func<RequestDelegate, RequestDelegate>>(x =>
                    x.Target != null
                    && x.Target.ToString() == typeof(ForwardedHeadersMiddleware).FullName
                )
            );
    }

    [Fact]
    public void UseAppForwardedHeaders_When_UseForwardedHeaders_Enabled_App_Registers_Forwarded_Headers()
    {
        var mockOptions = Substitute.For<IOptionsMonitor<ForwardedHeadersConfig>>();
        mockOptions.CurrentValue.Returns(new ForwardedHeadersConfig
        {
            Enable = true
        });
        var services = new ServiceCollection();
        services.AddSingleton(mockOptions);
        var mockApp = Substitute.For<IApplicationBuilder>();
        mockApp.ApplicationServices.Returns(services.BuildServiceProvider());

        mockApp.UseAppForwardedHeaders();

        Assert.Single(mockOptions.ReceivedCalls());
        mockApp
            .Received(1)
            .Use(
                Arg.Is<Func<RequestDelegate, RequestDelegate>>(x =>
                    x.Target != null
                    && x.Target.ToString() == typeof(ForwardedHeadersMiddleware).FullName
                )
            );
    }
}
