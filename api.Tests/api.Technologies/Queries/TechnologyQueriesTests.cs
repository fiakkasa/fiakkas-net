using api.Shared.Interfaces;
using api.Technologies.Interfaces;
using api.Technologies.Models;

namespace api.Technologies.Queries;

public class TechnologyQueriesTests
{
    [Fact]
    public void GetTechnologies_Should_Return_Result()
    {
        var item = new Technology
        {
            Id = Guid.Empty,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };
        var service = Substitute.For<IDataRepository<ITechnology>>();
        service
            .Get(Arg.Any<Func<ITechnology, Technology>>())
            .Returns(new[] { item }.AsQueryable());
        var qut = new TechnologyQueries();

        var result = qut.GetTechnologies(service);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<Technology>>();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
