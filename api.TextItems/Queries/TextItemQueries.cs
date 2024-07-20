using api.TextItems.DataLoaders;
using api.TextItems.Interfaces;
using api.TextItems.Mappers;
using api.TextItems.Models;

namespace api.TextItems.Queries;

[QueryType]
public static class TextItemQueries
{
    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<TextItem> GetTextItems([Service] IDataRepository<ITextItem> repository) =>
        repository.Get(TextItemMappers.Map);

    [NodeResolver]
    public static async ValueTask<TextItem?> GetTextItemById(
        Guid id,
        TextItemBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);
}
