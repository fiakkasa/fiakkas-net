using api.GraphExtensions.DataLoaders;

namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<PortfolioItem>]
public sealed class PortfolioItemTypeExtension
{
    public async ValueTask<PortfolioCategory?> GetCategory(
        [Parent] PortfolioItem parent,
        [Service] PortfolioCategoryBatchDataLoader dataLoader,
        CancellationToken cancellationToken
    ) => await dataLoader.LoadAsync(parent.CategoryId, cancellationToken);

    public async ValueTask<Customer?> GetCustomer(
        [Parent] PortfolioItem parent,
        [Service] CustomerBatchDataLoader dataLoader,
        CancellationToken cancellationToken
    ) => await dataLoader.LoadAsync(parent.CustomerId, cancellationToken);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async ValueTask<IEnumerable<IPolymorphicTechnologyCategory>> GetTechnologyCategories(
        [Parent] PortfolioItem parent,
        [Service] TechnologyCategoryGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        var result = await dataLoader.LoadAsync(parent.TechnologyIds, cancellationToken);

        return result.OfType<IPolymorphicTechnologyCategory[]>().SelectMany(x => x);
    }

    public async ValueTask<string> GetTechnologiesSummary(
        [Parent] PortfolioItem parent,
        [Service] TechnologyCategoryGroupDataLoader dataLoader,
        CancellationToken cancellationToken
    )
    {
        var result = await dataLoader.LoadAsync(parent.TechnologyIds, cancellationToken);

        return string.Join(
            ", ",
            result
                .OfType<IPolymorphicTechnologyCategory[]>()
                .SelectMany(x => x)
                .Select(x => x.Title)
        );
    }
}
