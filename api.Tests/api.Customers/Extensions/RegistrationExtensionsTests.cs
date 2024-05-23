using api.Customers.Interfaces;
using api.Customers.Models;
using api.Shared.Interfaces;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace api.Customers.Extensions.Tests;

public class RegistrationExtensionsTests
{
    [Fact]
    public void AddApiCustomers_Service_Registration_Should_Add_Options_And_Services()
    {
        var config = new Dictionary<string, object>
        {
            ["data"] = new CustomersDataConfig()
        }.GetConfigRoot();
        var serviceProvider =
            new ServiceCollection()
                .AddLogging()
                .AddApiCustomers(config)
                .BuildServiceProvider();

        var service = serviceProvider.GetService<IDataRepository<ICustomer>>();
        var options = serviceProvider.GetService<IOptionsSnapshot<CustomersDataConfig>>();

        service.Should().NotBeNull();
        options.Should().NotBeNull();
    }

    [Fact]
    public async Task AddApiCustomers_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(Substitute.For<IDataRepository<ICustomer>>())
                .AddGraphQLServer()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddApiCustomers()
                .BuildSchemaAsync();

        var schema = result.Print();

        schema.Should().NotBeEmpty();

        schema.MatchSnapshot();
    }
}
