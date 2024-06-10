using api.Languages.Interfaces;
using api.Languages.Models;

namespace api.Languages.Services;

public sealed class LanguageDataRepository(ILogger<LanguageDataRepository> logger, IOptionsSnapshot<LanguagesDataConfig> dataSnapshot)
: AbstractDataRepository<ILanguage, LanguagesDataConfig>(logger, dataSnapshot)
{
    protected override ILanguage[]? ResolveSet(LanguagesDataConfig data) => data.Languages;
}
