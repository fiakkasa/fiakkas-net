using api.Languages.Interfaces;
using api.Languages.Models;

namespace api.Languages.Services;

public sealed class LanguageDataRepository(ILogger<LanguageDataRepository> logger, IOptionsSnapshot<LanguagesDataConfig> dataSnapshot)
: InMemoryAbstractDataRepository<ILanguage, LanguagesDataConfig>(logger, dataSnapshot)
{
    protected override ILanguage[]? ResolveSet(LanguagesDataConfig data) => data.Languages;
}
