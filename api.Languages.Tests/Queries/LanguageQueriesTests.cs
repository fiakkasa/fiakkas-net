using api.Languages.DataLoaders;
using api.Languages.Enums;
using api.Languages.Interfaces;
using api.Languages.Models;
using api.Languages.Queries;
using GreenDonut;

namespace api.Languages.Tests.Queries;

public class LanguageQueriesTests
{
    [Fact]
    public void GetLanguages_Should_Return_Data()
    {
        var item = new Language
        {
            Id = new("02a3be9b-3f04-4b4a-8945-e84fef537b58"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Proficiency = ProficiencyType.Fluent,
            Title = "Title"
        };
        var dataRepository = new MockDataRepository<ILanguage>([item]);

        var result = LanguageQueries.GetLanguages(dataRepository);

        Assert.Single(result);
        Assert.IsAssignableFrom<IQueryable<Language>>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetLanguageById_Should_Return_Data_When_Found()
    {
        var id = new Guid("02a3be9b-3f04-4b4a-8945-e84fef537b58");
        var item = new Language
        {
            Id = id,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Proficiency = ProficiencyType.Fluent,
            Title = "Title"
        };
        var dataRepository = new MockDataRepository<ILanguage>([item]);
        var dataLoader = new LanguageBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await LanguageQueries.GetLanguageById(
            id,
            dataLoader,
            default
        );

        Assert.NotNull(result);
        Assert.IsType<Language>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetLanguageById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("02a3be9b-3f04-4b4a-8945-e84fef537b58");
        var dataRepository = new MockDataRepository<ILanguage>([]);
        var dataLoader = new LanguageBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await LanguageQueries.GetLanguageById(
            id,
            dataLoader,
            default
        );

        Assert.Null(result);
    }
}
