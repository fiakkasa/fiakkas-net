using api.Customers.Interfaces;
using api.Customers.Models;
using api.GraphExtensions.TestingShared;

namespace api.GraphExtensions.DataLoaders;

public class CustomerBatchDataLoaderTests
{
    [Fact]
    public async Task LoadBatchAsync_Should_Return_Data_When_Matches_Found()
    {
        var dataRepository = new MockDataRepository<ICustomer>(
        [
            new Customer
            {
                Id = Guid.Empty,
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                Title = "Title",
                Href = new Uri("/test", UriKind.Relative)
            }
        ]);

        var sut = new CustomerBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.Empty], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadBatchAsync_Should_Return_Empty_Collection_When_No_Matches_Found()
    {
        var dataRepository = new MockDataRepository<ICustomer>();

        var sut = new CustomerBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.Empty], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
