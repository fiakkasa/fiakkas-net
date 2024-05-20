using api.Portfolio.DataLoaders;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.TypeExtensions;

[ExtendObjectType<IPortfolioCategory>]
public class PortfolioCategoryTypeExtension
{
    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<PortfolioItem[]> GetPortfolioItems(
        [Parent] IPortfolioCategory parent,
        [Service] PortfolioItemByPortfolioCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);
}
