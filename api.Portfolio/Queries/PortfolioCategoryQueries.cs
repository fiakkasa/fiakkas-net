using api.Portfolio.Interfaces;
using api.Portfolio.Mappers;
using api.Portfolio.Models;

namespace api.Portfolio.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class PortfolioCategoryQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<PortfolioCategory> GetPortfolioCategories([Service] IDataRepository<IPortfolioCategory> repository) =>
        repository.Get(PortfolioCategoryMappers.Map);
}
