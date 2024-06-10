using api.Languages.Enums;

namespace api.Languages.Interfaces;

public interface ILanguage : IBaseData
{
    ProficiencyType Proficiency { get; init; }
    string Title { get; init; }
}
