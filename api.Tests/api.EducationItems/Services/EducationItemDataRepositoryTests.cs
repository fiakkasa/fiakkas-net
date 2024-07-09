using api.EducationItems.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.EducationItems.Services.Tests;

public class EducationItemDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new EducationItemEntity
        {
            Id = new Guid("38898c62-161e-40f2-8a9f-39bf1ff46224"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            CategoryId = new Guid("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
            TimePeriod = new()
            {
                Start = new DateOnly(2024, 1, 1),
                End = null
            },
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative),
            Location = "Location",
            Description = "Description",
            Subjects = ["Subject"]
        };
        var configData = new EducationItemsDataConfig
        {
            EducationItems = [item]
        };
        var configOptions = Substitute.For<IOptionsSnapshot<EducationItemsDataConfig>>();
        configOptions.Value.Returns(configData);

        var sut = new EducationItemDataRepository(Substitute.For<ILogger<EducationItemDataRepository>>(), configOptions);

        var result = sut.Get();

        result.Should().ContainSingle();
        result.MatchSnapshot();
    }
}
