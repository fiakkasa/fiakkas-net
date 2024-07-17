using api.EducationItems.Interfaces;
using api.EducationItems.Models;
using api.Shared.Types.Models;

namespace api.EducationItems.Mappers.Tests;

public class EducationItemMappersTests
{
    public record EducationItemMockEntity : IEducationItem
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public Guid CategoryId { get; init; }
        public TimePeriod TimePeriod { get; init; } = new();
        public string Title { get; init; } = string.Empty;
        public Uri? Href { get; init; }
        public string Location { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string[] Subjects { get; init; } = [];
    }

    [Fact]
    public void Map_Should_Return_Data()
    {
        var item = new EducationItemMockEntity
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

        var result = item.Map();

        result.Should().BeOfType<EducationItem>();
        result.MatchSnapshot();
    }
}
