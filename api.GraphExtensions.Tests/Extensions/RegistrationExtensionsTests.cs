using api.Categories.Interfaces;
using api.Categories.Models;
using api.Customers.Models;
using api.EducationItems.Models;
using api.GraphExtensions.Extensions;
using api.Portfolio.Models;
using api.Shared.Types.Interfaces;
using HotChocolate.Execution;
using HotChocolate.Types.Relay;

namespace api.GraphExtensions.Tests.Extensions;

public class RegistrationExtensionsTests
{
    [Fact]
    public async Task AddApiGraphExtensions_GraphQL_Registration_Should_Add_GraphQL_Assets()
    {
        var result =
            await new ServiceCollection()
                .AddSingleton(typeof(IDataRepository<>), typeof(MockDataRepository<>))
                .AddGraphQLServer()
                .AddGlobalObjectIdentification()
                .AddQueryType()
                .AddSorting()
                .AddFiltering()
                .AddInterfaceType<IPolymorphicTechnologyCategory>(descriptor => descriptor.Field(f => f.Id).ID())
                .AddTypeExtension(typeof(TestQueries))
                .AddObjectType<MockTechnologyCategory>()
                .AddApiGraphExtensions()
                .BuildSchemaAsync();

        var schema = result.Print();

        Assert.NotEmpty(schema);
        schema.MatchSnapshot();
    }

    [Node]
    public record MockTechnologyCategory : ITechnologyCategory, IPolymorphicTechnologyCategory
    {
        [ID]
        public Guid Id { get; init; }

        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public string Title { get; init; } = string.Empty;
        public Uri? Href { get; init; }
    }

    [QueryType]
    public static class TestQueries
    {
        public static IEnumerable<Customer> Customers => [];
        public static IEnumerable<EducationItem> EducationItems => [];
        public static IEnumerable<IPolymorphicTechnologyCategory> PolymorphicTechnologyCategories => [];
        public static IEnumerable<MockTechnologyCategory> TechnologyCategories => [];
        public static IEnumerable<PortfolioCategory> PortfolioCategories => [];
        public static IEnumerable<PortfolioItem> PortfolioItems => [];
        public static IEnumerable<ResumeCategory> ResumeCategories => [];

        [NodeResolver]
        public static MockTechnologyCategory? GetMockTechnologyCategoryById(Guid id) => default;
    }
}
