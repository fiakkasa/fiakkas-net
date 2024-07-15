using api.TextItems.Interfaces;
using api.TextItems.Mappers;
using api.TextItems.Models;

namespace api.TextItems.Queries;

[QueryType]
public sealed class TextItemQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<TextItem> GetTextItems([Service] IDataRepository<ITextItem> repository) =>
        repository.Get(TextItemMappers.Map);
}
