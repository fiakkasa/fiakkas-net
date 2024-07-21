using api.Languages.Enums;
using api.Languages.Interfaces;

namespace api.Languages.Models;

[ExcludeFromCodeCoverage]
public record Language : AbstractBaseData, ILanguage
{
    public ProficiencyType Proficiency { get; init; }
    public string Title { get; init; } = string.Empty;
}
