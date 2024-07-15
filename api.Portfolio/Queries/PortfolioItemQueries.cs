using api.Portfolio.Interfaces;
using api.Portfolio.Mappers;
using api.Portfolio.Models;

namespace api.Portfolio.Queries;

[QueryType]
public sealed class PortfolioItemQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<PortfolioItem> GetPortfolioItems([Service] IDataRepository<IPortfolioItem> repository) =>
        repository.Get(PortfolioItemMappers.Map);
}
