using api.ContactItems.Interfaces;
using api.ContactItems.Models;
using api.Shared.Interfaces;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace api.ContactItems.Extensions.Tests;

public class RegistrationExtensionsTests
{
    [Fact]
    public void AddApiContactItems_Service_Registration_Should_Add_Options_And_Services()
    {
        var configuration = new Dictionary<string, object>
        {
            ["data"] = new ContactItemsDataConfig()
        }.ToConfiguration();
        var serviceProvider =
            new ServiceCollection()
                .AddLogging()
                .AddApiContactItems(configuration)
                .BuildServiceProvider();

        var dataRepository = serviceProvider.GetService<IDataRepository<IContactItem>>();
        var options = serviceProvider.GetService<IOptionsSnapshot<ContactItemsDataConfig>>();

        dataRepository.Should().NotBeNull();
        options.Should().NotBeNull();
    }

    [Fact]
    public async Task AddApiContactItems_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(Substitute.For<IDataRepository<IContactItem>>())
                .AddGraphQLServer()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddApiContactItems()
                .BuildSchemaAsync();

        var schema = result.Print();

        schema.Should().NotBeEmpty();

        schema.MatchSnapshot();
    }
}
