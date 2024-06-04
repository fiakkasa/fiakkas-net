using api.Categories.Interfaces;
using api.Categories.Models;

namespace api.Categories.Queries;

public class CategoryQueriesTests
{
    [Fact]
    public void GetCategories_Should_Return_Data()
    {
        var item = new Category
        {
            Id = new Guid("38e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title"
        };
        var dataRepository = new MockDataRepository<ICategory>([item]);
        var sut = new CategoryQueries();

        var result = sut.GetCategories(dataRepository);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<Category>>();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
