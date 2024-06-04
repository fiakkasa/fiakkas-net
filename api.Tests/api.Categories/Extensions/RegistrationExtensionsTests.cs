using api.Categories.Interfaces;
using api.Categories.Models;
using api.Shared.Interfaces;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace api.Categories.Extensions.Tests;

public class RegistrationExtensionsTests
{
    [Fact]
    public void AddApiCategories_Service_Registration_Should_Add_Options_And_Services()
    {
        var config = new Dictionary<string, object>
        {
            ["data"] = new CategoriesDataConfig()
        }.GetConfigRoot();
        var serviceProvider =
            new ServiceCollection()
                .AddLogging()
                .AddApiCategories(config)
                .BuildServiceProvider();

        var dataRepository = serviceProvider.GetService<IDataRepository<ICategory>>();
        var options = serviceProvider.GetService<IOptionsSnapshot<CategoriesDataConfig>>();

        dataRepository.Should().NotBeNull();
        options.Should().NotBeNull();
    }

    [Fact]
    public async Task AddApiCategories_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(Substitute.For<IDataRepository<ICategory>>())
                .AddGraphQLServer()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddApiCategories()
                .BuildSchemaAsync();

        var schema = result.Print();

        schema.Should().NotBeEmpty();

        schema.MatchSnapshot();
    }
}
