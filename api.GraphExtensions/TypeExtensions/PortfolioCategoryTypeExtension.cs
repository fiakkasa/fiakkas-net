using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<PortfolioCategory>]
public sealed class PortfolioCategoryTypeExtension
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<Customer[]?> GetCustomers(
        [Parent] PortfolioCategory parent,
        [Service] CustomerByPortfolioCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<PortfolioItem[]?> GetPortfolioItems(
        [Parent] PortfolioCategory parent,
        [Service] PortfolioItemByPortfolioCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<IPolymorphicTechnologyCategory[]?> GetTechnologyCategories(
        [Parent] PortfolioCategory parent,
        [Service] PortfolioTechnologyCategoryByPortfolioCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);
}
