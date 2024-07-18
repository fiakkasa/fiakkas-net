using api.Languages.Enums;
using api.Languages.Interfaces;
using api.Languages.Models;

namespace api.Languages.Queries.Tests;

public class LanguageQueriesTests
{
    [Fact]
    public void GetLanguages_Should_Return_Data()
    {
        var item = new Language
        {
            Id = new Guid("02a3be9b-3f04-4b4a-8945-e84fef537b58"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Proficiency = ProficiencyType.Fluent,
            Title = "Title"
        };
        var dataRepository = new MockDataRepository<ILanguage>([item]);

        var result = LanguageQueries.GetLanguages(dataRepository);

        result.Should().ContainSingle();
        result.Should().BeAssignableTo<IQueryable<Language>>();
        result.MatchSnapshot();
    }
}