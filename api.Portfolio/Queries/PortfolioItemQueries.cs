using api.Portfolio.DataLoaders;
using api.Portfolio.Interfaces;
using api.Portfolio.Mappers;
using api.Portfolio.Models;

namespace api.Portfolio.Queries;

[QueryType]
public static class PortfolioItemQueries
{
    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<PortfolioItem> GetPortfolioItems([Service] IDataRepository<IPortfolioItem> repository) =>
        repository.Get(PortfolioItemMappers.Map);

    [NodeResolver]
    public static async ValueTask<PortfolioItem?> GetPortfolioItemById(
        Guid id,
        PortfolioItemBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);
}
