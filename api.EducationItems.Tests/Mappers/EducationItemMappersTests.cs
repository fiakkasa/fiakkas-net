using api.EducationItems.Interfaces;
using api.EducationItems.Mappers;
using api.EducationItems.Models;
using api.Shared.Types.Models;

namespace api.EducationItems.Tests.Mappers;

public class EducationItemMappersTests
{
    [Fact]
    public void Map_Should_Return_Data()
    {
        var item = new EducationItemMockEntity
        {
            Id = new("38898c62-161e-40f2-8a9f-39bf1ff46224"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            CategoryId = new("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
            TimePeriod = new()
            {
                Start = new(2024, 1, 1),
                End = null
            },
            Title = "Title",
            Href = new("/test", UriKind.Relative),
            Location = "Location",
            Description = "Description",
            Subjects = ["Subject"]
        };

        var result = item.Map();

        result.Should().BeOfType<EducationItem>();
        result.MatchSnapshot();
    }

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
}
