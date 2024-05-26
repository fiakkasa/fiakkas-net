using api.Customers.Models;
using api.Portfolio.Models;
using api.Shared.Interfaces;
using api.Technologies.Models;
using HotChocolate.Execution;
using HotChocolate.Language;
using Microsoft.Extensions.DependencyInjection;

namespace api.GraphExtensions.Extensions.Tests;

public class RegistrationExtensionsTests
{
    [ExtendObjectType(OperationType.Query)]
    public class TestQueries
    {
        public IEnumerable<Customer> Customers => [];
        public IEnumerable<PortfolioItem> PortfolioItems => [];
        public IEnumerable<PortfolioCategory> PortfolioCategories => [];
        public IEnumerable<Technology> Technologies => [];
    }

    [Fact]
    public async Task AddApiCustomers_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(typeof(IDataRepository<>), typeof(MockDataRepository<>))
                .AddGraphQLServer()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddTypeExtension<TestQueries>()
                .AddApiGraphExtensions()
                .BuildSchemaAsync();

        var schema = result.Print();

        schema.Should().NotBeEmpty();

        schema.MatchSnapshot();
    }
}
