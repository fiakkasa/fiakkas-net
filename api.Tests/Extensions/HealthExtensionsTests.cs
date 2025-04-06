using api.Extensions;

namespace api.Tests.Extensions;

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

        Assert.NotNull(options);

        var result =
            options!
                .CurrentValue
                .Registrations
                .Select(x => x.Name)
                .ToArray();

        Assert.Contains(result, x => x == Consts.ApiHealthName);
        Assert.Contains(result, x => x == Consts.GraphQLHealthName);
        result.MatchSnapshot();
    }
}
