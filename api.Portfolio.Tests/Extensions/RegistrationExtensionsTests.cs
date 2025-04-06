using api.Portfolio.Extensions;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Shared.Types.Interfaces;
using HotChocolate.Execution;

namespace api.Portfolio.Tests.Extensions;

public class RegistrationExtensionsTests
{
    [Fact]
    public void AddApiPortfolio_Service_Registration_Should_Add_Options_And_Services()
    {
        var configuration = new Dictionary<string, object>
        {
            ["data"] = new PortfolioDataConfig()
        }.ToConfiguration();
        var serviceProvider =
            new ServiceCollection()
                .AddSingleton(configuration)
                .AddLogging()
                .AddApiPortfolio()
                .BuildServiceProvider();

        var dataRepository = serviceProvider.GetService<IDataRepository<IPortfolioItem>>();
        var options = serviceProvider.GetService<IOptionsSnapshot<PortfolioDataConfig>>();

        dataRepository.Should().NotBeNull();
        options.Should().NotBeNull();
    }

    [Fact]
    public async Task AddApiPortfolio_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(Substitute.For<IDataRepository<IPortfolioItem>>())
                .AddGraphQLServer()
                .AddGlobalObjectIdentification()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddApiPortfolio()
                .BuildSchemaAsync();

        var schema = result.Print();

        schema.Should().NotBeEmpty();
        schema.MatchSnapshot();
    }
}
