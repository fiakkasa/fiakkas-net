using api.Languages.Interfaces;
using api.Languages.Mappers;
using api.Languages.Models;

namespace api.Languages.Queries;

[QueryType]
public sealed class LanguageQueries
{
    [UseOffsetPaging]
    [UseSorting]
    [UseFiltering]
    public IQueryable<Language> GetLanguages([Service] IDataRepository<ILanguage> repository) =>
        repository.Get(LanguageMappers.Map);
}
