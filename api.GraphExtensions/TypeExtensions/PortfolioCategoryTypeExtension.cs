using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<IPortfolioCategory>]
public class PortfolioCategoryTypeExtension
{
    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<Customer[]> GetCustomers(
        [Parent] IPortfolioCategory parent,
        [Service] CustomerByPortfolioCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);

    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<Technology[]> GetTechnologies(
        [Parent] IPortfolioCategory parent,
        [Service] TechnologyByPortfolioCategoryIdGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.Id, cancellationToken);
}
