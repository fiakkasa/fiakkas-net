using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<Customer>]
public sealed class CustomerTypeExtension
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<PortfolioCategory[]?> GetPortfolioCategories(
        [Parent] Customer parent,
        [Service] PortfolioCategoryByCustomerIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<PortfolioItem[]?> GetPortfolioItems(
        [Parent] Customer parent,
        [Service] PortfolioItemByCustomerIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<IPolymorphicTechnologyCategory[]?> GetPortfolioTechnologyCategories(
        [Parent] Customer parent,
        [Service] PortfolioTechnologyCategoryByCustomerIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);
}
