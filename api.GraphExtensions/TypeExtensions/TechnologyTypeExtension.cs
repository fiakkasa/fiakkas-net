using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<ITechnology>]
public class TechnologyTypeExtension
{
    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<PortfolioCategory[]> GetPortfolioCategories(
        [Parent] ITechnology parent,
        [Service] PortfolioCategoryByTechnologyIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<Customer[]> GetCustomers(
        [Parent] ITechnology parent,
        [Service] CustomerByTechnologyIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<IPortfolioItem[]> GetPortfolioItems(
        [Parent] ITechnology parent,
        [Service] PortfolioItemByTechnologyIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);
}
