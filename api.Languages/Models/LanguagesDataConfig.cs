namespace api.Languages.Models;

[ExcludeFromCodeCoverage]
public record LanguagesDataConfig
{
    public LanguageEntity[] Languages { get; init; } = [];
}
