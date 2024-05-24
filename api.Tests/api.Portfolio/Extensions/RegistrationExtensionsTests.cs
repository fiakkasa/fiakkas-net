using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Shared.Interfaces;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace api.Portfolio.Extensions.Tests;

public class RegistrationExtensionsTests
{
    [Fact]
    public void AddApiPortfolio_Service_Registration_Should_Add_Options_And_Services()
    {
        var config = new Dictionary<string, object>
        {
            ["data"] = new PortfolioDataConfig()
        }.GetConfigRoot();
        var serviceProvider =
            new ServiceCollection()
                .AddLogging()
                .AddApiPortfolio(config)
                .BuildServiceProvider();

        var portfolioItemDataRepository = serviceProvider.GetService<IDataRepository<IPortfolioItem>>();
        var portfolioCategoryDataRepository = serviceProvider.GetService<IDataRepository<IPortfolioCategory>>();
        var options = serviceProvider.GetService<IOptionsSnapshot<PortfolioDataConfig>>();

        portfolioItemDataRepository.Should().NotBeNull();
        portfolioCategoryDataRepository.Should().NotBeNull();
        options.Should().NotBeNull();
    }

    [Fact]
    public async Task AddApiPortfolio_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(Substitute.For<IDataRepository<IPortfolioItem>>())
                .AddSingleton(Substitute.For<IDataRepository<IPortfolioCategory>>())
                .AddGraphQLServer()
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
