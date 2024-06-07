using api.Categories.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.Categories.Services.Tests;

public class CategoryDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new CategoryEntity
        {
            Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };
        var configData = new CategoriesDataConfig
        {
            Categories = [item]
        };
        var configOptions = Substitute.For<IOptionsSnapshot<CategoriesDataConfig>>();
        configOptions.Value.Returns(configData);

        var sut = new CategoryDataRepository(Substitute.For<ILogger<CategoryDataRepository>>(), configOptions);

        var result = sut.Get();

        result.Should().NotBeEmpty();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
