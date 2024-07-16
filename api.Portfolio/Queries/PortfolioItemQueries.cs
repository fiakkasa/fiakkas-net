using api.Portfolio.Interfaces;
using api.Portfolio.Mappers;
using api.Portfolio.Models;

namespace api.Portfolio.Queries;

[QueryType]
public static class PortfolioItemQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<PortfolioItem> GetPortfolioItems([Service] IDataRepository<IPortfolioItem> repository) =>
        repository.Get(PortfolioItemMappers.Map);
}
