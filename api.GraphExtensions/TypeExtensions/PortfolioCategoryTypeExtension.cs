using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<PortfolioCategory>]
public sealed class PortfolioCategoryTypeExtension
{
    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<Customer[]> GetCustomers(
        [Parent] PortfolioCategory parent,
        [Service] CustomerByPortfolioCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<PortfolioItem[]> GetPortfolioItems(
        [Parent] PortfolioCategory parent,
        [Service] PortfolioItemByPortfolioCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<ITechnologyCategory[]> GetTechnologyCategories(
        [Parent] PortfolioCategory parent,
        [Service] PortfolioTechnologyCategoryByPortfolioCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);
}
