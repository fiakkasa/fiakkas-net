using api.Portfolio.Extensions;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;
using api.Shared.Interfaces;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace api.Portfolio.Tests;

public class GraphFixture
{
    private IRequestExecutor? _requestExecutor;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async ValueTask<IRequestExecutor> GetRequestExecutor()
    {
        if (_requestExecutor is not { })
        {
            _semaphore.Wait();

            var portfolioItems = new[]
            {
                new PortfolioItem
                {
                    Id = Guid.Empty,
                    CreatedAt = new (2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title",
                    Href = new Uri("/test", UriKind.Relative),
                    TechnologyIds = [Guid.Empty],
                    CustomerId = Guid.Empty
                }
            };
            var portfolioItemDataRepository = Substitute.For<IDataRepository<IPortfolioItem>>();
            portfolioItemDataRepository
                .Get(Arg.Any<Func<IPortfolioItem, PortfolioItem>>())
                .Returns(portfolioItems.AsQueryable());
            portfolioItemDataRepository
                .GetBatch(
                    Arg.Any<IReadOnlyList<Guid>>(),
                    Arg.Any<Func<IPortfolioItem, PortfolioItem>>(),
                    Arg.Any<CancellationToken>()
                )
                .Returns(portfolioItems.ToDictionary(x => x.Id));
            portfolioItemDataRepository
                .GetGroupedBatch(
                    Arg.Any<IReadOnlyList<Guid>>(),
                    Arg.Any<Func<IPortfolioItem, Guid>>(),
                    Arg.Any<Func<IPortfolioItem, PortfolioItem>>(),
                    Arg.Any<CancellationToken>()
                )
                .Returns(portfolioItems.ToLookup(x => x.Id));

            var portfolioCategories = new[]
            {
                new PortfolioCategory
                {
                    Id = Guid.Empty,
                    CreatedAt = new (2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    UpdatedAt = null,
                    Version = 1,
                    Title = "Title",
                    Href = new Uri("/test", UriKind.Relative)
                }
            };
            var portfolioCategoryDataRepository = Substitute.For<IDataRepository<IPortfolioCategory>>();
            portfolioCategoryDataRepository
                .Get(Arg.Any<Func<IPortfolioCategory, PortfolioCategory>>())
                .Returns(portfolioCategories.AsQueryable());
            portfolioCategoryDataRepository
                .GetBatch(
                    Arg.Any<IReadOnlyList<Guid>>(),
                    Arg.Any<Func<IPortfolioCategory, PortfolioCategory>>(),
                    Arg.Any<CancellationToken>()
                )
                .Returns(portfolioCategories.ToDictionary(x => x.Id));
            portfolioCategoryDataRepository
                .GetGroupedBatch(
                    Arg.Any<IReadOnlyList<Guid>>(),
                    Arg.Any<Func<IPortfolioCategory, Guid>>(),
                    Arg.Any<Func<IPortfolioCategory, PortfolioCategory>>(),
                    Arg.Any<CancellationToken>()
                )
                .Returns(portfolioCategories.ToLookup(x => x.Id));
            _requestExecutor =
                await new ServiceCollection()
                    .AddSingleton(portfolioItemDataRepository)
                    .AddSingleton(portfolioCategoryDataRepository)
                    .AddGraphQL()
                    .AddQueryType()
                    .AddFiltering()
                    .AddSorting()
                    .AddApiPortfolio()
                    .BuildRequestExecutorAsync();

            _semaphore.Release();
        }

        return _requestExecutor;
    }
}
