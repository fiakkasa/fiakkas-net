using api.Languages.DataLoaders;
using api.Languages.Interfaces;
using api.Languages.Mappers;
using api.Languages.Models;

namespace api.Languages.Queries;

[QueryType]
public static class LanguageQueries
{
    [UsePaging]
    [UseSorting]
    [UseFiltering]
    public static IQueryable<Language> GetLanguages([Service] IDataRepository<ILanguage> repository) =>
        repository.Get(LanguageMappers.Map);

    [NodeResolver]
    public static async ValueTask<Language?> GetLanguageById(
        Guid id,
        LanguageBatchDataLoader dataLoader,
        CancellationToken cancellationToken = default
    ) =>
        await dataLoader.LoadAsync(id, cancellationToken);
}
