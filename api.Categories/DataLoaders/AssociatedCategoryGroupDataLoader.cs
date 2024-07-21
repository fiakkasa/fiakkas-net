using api.Categories.Enums;
using api.Categories.Interfaces;
using api.Categories.Mappers;

namespace api.Categories.DataLoaders;

public sealed class AssociatedCategoryGroupDataLoader(
    IDataRepository<ICategory> dataRepository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null
) : GroupedDataLoader<CategoryType, IPolymorphicCategory>(batchScheduler, options)
{
    protected override async Task<ILookup<CategoryType, IPolymorphicCategory>> LoadGroupedBatchAsync(
        IReadOnlyList<CategoryType> keys,
        CancellationToken cancellationToken
    ) =>
        await dataRepository.GetGroupedBatch(
            x => keys.Contains(x.Kind),
            x => x.Kind,
            CategoryMappers.MapPolymorphicCategory,
            cancellationToken
        );
}
