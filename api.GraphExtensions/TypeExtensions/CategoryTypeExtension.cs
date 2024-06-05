using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<ICategory>]
public sealed class CategoryTypeExtension
{
    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<Customer[]> GetPortfolioCustomers(
        [Parent] ICategory parent,
        [Service] CustomerByPortfolioCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<PortfolioItem[]> GetPortfolioItems(
        [Parent] ICategory parent,
        [Service] PortfolioItemByPortfolioCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<Technology[]> GetPortfolioTechnologies(
        [Parent] ICategory parent,
        [Service] TechnologyByPortfolioCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);
}
