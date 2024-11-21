using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<ITechnologyCategory>]
public sealed class ITechnologyCategoryTypeExtension
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<PortfolioCategory[]?> GetPortfolioCategories(
        [Parent] ITechnologyCategory parent,
        [Service] PortfolioCategoryByTechnologyIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<Customer[]?> GetPortfolioCustomers(
        [Parent] ITechnologyCategory parent,
        [Service] CustomerByTechnologyIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<PortfolioItem[]?> GetPortfolioItems(
        [Parent] ITechnologyCategory parent,
        [Service] PortfolioItemByTechnologyIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);
}
