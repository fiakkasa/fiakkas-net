using api.Portfolio.DataLoaders;
using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.TypeExtensions;

[ExtendObjectType<IPortfolioItem>]
public class PortfolioItemTypeExtension
{
    [BindMember(nameof(IPortfolioItem.CategoryId))]
    public async ValueTask<PortfolioCategory?> GetCategory(
        [Parent] IPortfolioItem parent,
        [Service] PortfolioCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.CategoryId, cancellationToken);
}
