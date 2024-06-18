using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<PortfolioItem>]
public sealed class PortfolioItemTypeExtension
{
    public async ValueTask<PortfolioCategory?> GetCategory(
        [Parent] PortfolioItem parent,
        [Service] PortfolioCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.CategoryId, cancellationToken);

    public async ValueTask<Customer?> GetCustomer(
        [Parent] PortfolioItem parent,
        [Service] CustomerBatchDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.CustomerId, cancellationToken);

    [UseOffsetPaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<IReadOnlyList<ITechnologyCategory>> GetTechnologyCategories(
        [Parent] PortfolioItem parent,
        [Service] TechnologyCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        await dataLoader.LoadAsync(parent.TechnologyIds, cancellationToken);

    public async ValueTask<string> GetTechnologiesSummary(
        [Parent] PortfolioItem parent,
        [Service] TechnologyCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken
    ) =>
        string.Join(
            ", ",
            (await dataLoader.LoadAsync(parent.TechnologyIds, cancellationToken)).OfType<ITechnologyCategory>().Select(x => x.Title)
        );
}
