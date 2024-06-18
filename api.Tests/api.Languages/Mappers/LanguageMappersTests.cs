using api.Languages.Enums;
using api.Languages.Interfaces;
using api.Languages.Models;

namespace api.Languages.Mappers.Tests;

public class LanguageMappersTests
{
    public record LanguageMockEntity : ILanguage
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public ProficiencyType Proficiency { get; init; }
        public string Title { get; init; } = string.Empty;
    }

    [Fact]
    public void Map_Should_Return_Data()
    {
        var item = new LanguageMockEntity
        {
            Id = new Guid("02a3be9b-3f04-4b4a-8945-e84fef537b58"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Proficiency = ProficiencyType.Fluent,
            Title = "Title"
        };

        var result = item.Map();

        result.Should().BeOfType<Language>();
        result.MatchSnapshot();
    }
}
