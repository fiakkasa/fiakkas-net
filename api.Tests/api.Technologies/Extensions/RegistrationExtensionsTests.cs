using api.Shared.Interfaces;
using api.Technologies.Interfaces;
using api.Technologies.Models;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace api.Technologies.Extensions.Tests;

public class RegistrationExtensionsTests
{
    [Fact]
    public void AddApiTechnologies_Service_Registration_Should_Add_Options_And_Services()
    {
        var config = new Dictionary<string, object>
        {
            ["data"] = new TechnologiesDataConfig()
        }.GetConfigRoot();
        var serviceProvider =
            new ServiceCollection()
                .AddLogging()
                .AddApiTechnologies(config)
                .BuildServiceProvider();

        var service = serviceProvider.GetService<IDataRepository<ITechnology>>();
        var options = serviceProvider.GetService<IOptionsSnapshot<TechnologiesDataConfig>>();

        service.Should().NotBeNull();
        options.Should().NotBeNull();
    }

    [Fact]
    public async Task AddApiTechnologies_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(Substitute.For<IDataRepository<ITechnology>>())
                .AddGraphQLServer()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddApiTechnologies()
                .BuildSchemaAsync();

        var schema = result.Print();

        schema.Should().NotBeEmpty();

        schema.MatchSnapshot();
    }
}
