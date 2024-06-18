using api.Languages.Enums;
using api.Languages.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.Languages.Services.Tests;

public class LanguageDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new LanguageEntity
        {
            Id = new Guid("02a3be9b-3f04-4b4a-8945-e84fef537b58"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Proficiency = ProficiencyType.Fluent,
            Title = "Title"
        };
        var configData = new LanguagesDataConfig
        {
            Languages = [item]
        };
        var configOptions = Substitute.For<IOptionsSnapshot<LanguagesDataConfig>>();
        configOptions.Value.Returns(configData);

        var sut = new LanguageDataRepository(Substitute.For<ILogger<LanguageDataRepository>>(), configOptions);

        var result = sut.Get();

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
