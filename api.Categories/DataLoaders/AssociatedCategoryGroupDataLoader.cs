using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Mappers;

namespace api.Categories.DataLoaders;

public sealed class AssociatedCategoryGroupDataLoader(
    IDataRepository<ICategoryEntity> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<CategoryType, ICategory>(batchScheduler, options)
{
    protected override async Task<ILookup<CategoryType, ICategory>> LoadGroupedBatchAsync(
        IReadOnlyList<CategoryType> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetGroupedBatch(
            x => keys.Contains(x.Kind),
            x => x.Kind,
            CategoryMappers.Map,
            cancellationToken
        );
}
