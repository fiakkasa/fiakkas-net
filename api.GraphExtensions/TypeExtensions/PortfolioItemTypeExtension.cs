using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<IPortfolioItem>]
public class PortfolioItemTypeExtension
{
    public async ValueTask<Category?> GetCategory(
        [Parent] IPortfolioItem parent,
        [Service] CategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.CategoryId, cancellationToken);

    public async ValueTask<Customer?> GetCustomer(
        [Parent] IPortfolioItem parent,
        [Service] CustomerBatchDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.CustomerId, cancellationToken);

    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<IReadOnlyList<Technology>> GetTechnologies(
        [Parent] IPortfolioItem parent,
        [Service] TechnologyBatchDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.TechnologyIds, cancellationToken);

    public async ValueTask<string> GetTechnologiesSummary(
        [Parent] IPortfolioItem parent,
        [Service] TechnologyBatchDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        string.Join(
            ", ",
            (await dataLoader.LoadAsync(parent.TechnologyIds, cancellationToken)).Select(x => x.Title)
        );
}
