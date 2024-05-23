using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace api.Extensions.Tests;

public class HealthExtensionsTests
{
    [Fact]
    public void AddApiHealth_Adds_Health_Checks()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddApiHealth();

        var result =
            serviceCollection
                .BuildServiceProvider()
                .GetRequiredService<IOptionsMonitor<HealthCheckServiceOptions>>()
                .CurrentValue
                .Registrations.Select(x => x.Name)
                .ToArray();

        result.Should().HaveCount(2);
        result.Should().Contain(Consts.ApiHealthName);
        result.Should().Contain(Consts.GraphQLHealthName);
        result.MatchSnapshot();
    }
}
