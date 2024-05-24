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
    public class MockDataRepository<T> : IDataRepository<T> where T : IBaseId
    {
        public IQueryable<T> Get() => throw new NotImplementedException();

        public IQueryable<TMapped> Get<TMapped>(Func<T, TMapped> mapper)
            => throw new NotImplementedException();
        
        public ValueTask<IReadOnlyDictionary<Guid, TMapped>> GetBatch<TMapped>(
            IReadOnlyList<Guid> keys,
            Func<T, TMapped> mapper,
            CancellationToken cancellationToken = default
        ) where TMapped : IBaseId
        => throw new NotImplementedException();

        public ValueTask<ILookup<Guid, TMapped>> GetGroupedBatch<TMapped>(
            IReadOnlyList<Guid> keys, 
            Func<T, Guid> keySelector, 
            Func<T, TMapped> mapper, 
            CancellationToken cancellationToken = default
        ) => throw new NotImplementedException();
    }

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
