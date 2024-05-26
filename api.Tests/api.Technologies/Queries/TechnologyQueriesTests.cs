using api.Technologies.Interfaces;
using api.Technologies.Models;

namespace api.Technologies.Queries;

public class TechnologyQueriesTests
{
    [Fact]
    public void GetTechnologies_Should_Return_Data()
    {
        var item = new Technology
        {
            Id = new Guid("48e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };
        var dataRepository = new MockDataRepository<ITechnology>([item]);

        var sut = new TechnologyQueries();

        var result = sut.GetTechnologies(dataRepository);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<Technology>>();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
