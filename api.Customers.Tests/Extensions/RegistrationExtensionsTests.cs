using api.Customers.Extensions;
using api.Customers.Interfaces;
using api.Customers.Models;
using api.Shared.Types.Interfaces;
using HotChocolate.Execution;

namespace api.Customers.Tests.Extensions;

public class RegistrationExtensionsTests
{
    [Fact]
    public void AddApiCustomers_Service_Registration_Should_Add_Options_And_Services()
    {
        var configuration = new Dictionary<string, object>
        {
            ["data"] = new CustomersDataConfig()
        }.ToConfiguration();
        var serviceProvider =
            new ServiceCollection()
                .AddSingleton(configuration)
                .AddLogging()
                .AddApiCustomers()
                .BuildServiceProvider();

        var dataRepository = serviceProvider.GetService<IDataRepository<ICustomer>>();
        var options = serviceProvider.GetService<IOptionsSnapshot<CustomersDataConfig>>();

        Assert.NotNull(dataRepository);
        Assert.NotNull(options);
    }

    [Fact]
    public async Task AddApiCustomers_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(Substitute.For<IDataRepository<ICustomer>>())
                .AddGraphQLServer()
                .AddGlobalObjectIdentification()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddApiCustomers()
                .BuildSchemaAsync();

        var schema = result.Print();

        Assert.NotEmpty(schema);
        schema.MatchSnapshot();
    }
}
