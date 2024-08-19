using api.Languages.Interfaces;
using api.Languages.Models;

namespace api.Languages.Services;

public sealed class LanguageDataRepository(
    ILogger<LanguageDataRepository> logger,
    IOptionsSnapshot<LanguagesDataConfig> dataSnapshot
) : AbstractReadOnlyInMemoryDataRepository<ILanguage, LanguagesDataConfig>(logger, dataSnapshot)
{
    protected override IReadOnlyCollection<ILanguage>? ResolveSet(LanguagesDataConfig data) => data.Languages;
}
