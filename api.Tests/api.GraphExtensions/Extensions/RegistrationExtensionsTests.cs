using api.Categories.Interfaces;
using api.Categories.Models;
using api.Customers.Models;
using api.Portfolio.Models;
using api.Shared.Interfaces;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace api.GraphExtensions.Extensions.Tests;

public class RegistrationExtensionsTests
{
    public record MockTechnologyCategory : ITechnologyCategory
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public string Title { get; init; } = string.Empty;
        public Uri? Href { get; init; }
    }

    [QueryType]
    public class TestQueries
    {
        public IEnumerable<Customer> Customers => [];
        public IEnumerable<ITechnologyCategory> TechnologyCategories => [];
        public IEnumerable<PortfolioCategory> PortfolioCategories => [];
        public IEnumerable<PortfolioItem> PortfolioItems => [];
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
                .AddObjectType<MockTechnologyCategory>()
                .AddApiGraphExtensions()
                .BuildSchemaAsync();

        var schema = result.Print();

        schema.Should().NotBeEmpty();

        schema.MatchSnapshot();
    }
}
