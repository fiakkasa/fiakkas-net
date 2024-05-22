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

        Assert.Equal(2, result.Length);
        Assert.Contains(Consts.ApiHealthName, result);
        Assert.Contains(Consts.GraphQLHealthName, result);
        result.MatchSnapshot();
    }
}
