namespace api.Extensions.Tests;

public class HealthExtensionsTests
{
    [Fact]
    public void AddApiHealth_Should_Add_Health_Checks()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddApiHealth();

        var options =
            serviceCollection
                .BuildServiceProvider()
                .GetService<IOptionsMonitor<HealthCheckServiceOptions>>();

        options.Should().NotBeNull();

        var result =
            options!
                .CurrentValue
                .Registrations.Select(x => x.Name)
                .ToArray();

        result.Should().Contain(Consts.ApiHealthName);
        result.Should().Contain(Consts.GraphQLHealthName);
        result.MatchSnapshot();
    }
}
