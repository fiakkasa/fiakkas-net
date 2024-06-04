using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<ICustomer>]
public class CustomerTypeExtension
{
    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<Category[]> GetPortfolioCategories(
        [Parent] ICustomer parent,
        [Service] PortfolioCategoryByCustomerIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<PortfolioItem[]> GetPortfolioItems(
        [Parent] ICustomer parent,
        [Service] PortfolioItemByCustomerIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<Technology[]> GetPortfolioTechnologies(
        [Parent] ICustomer parent,
        [Service] TechnologyByCustomerIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);
}
