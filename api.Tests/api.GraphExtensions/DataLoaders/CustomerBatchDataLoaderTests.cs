using api.Customers.Interfaces;
using api.Customers.Models;
using api.Shared.Interfaces;

namespace api.GraphExtensions.DataLoaders;

public class CustomerBatchDataLoaderTests
{
    [Fact]
    public async Task LoadBatchAsync_Should_Return_Data_When_Matches_Found()
    {
        var dataRepository = Substitute.For<IDataRepository<ICustomer>>();

        dataRepository
            .GetBatch(Arg.Any<IReadOnlyList<Guid>>(), Arg.Any<Func<ICustomer, Customer>>(), Arg.Any<CancellationToken>())
            .Returns(
                new[]
                {
                    new Customer
                    {
                        Id = Guid.Empty,
                        CreatedAt = new (2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                        UpdatedAt = null,
                        Version = 1,
                        Title = "Title",
                        Href = new Uri("/test", UriKind.Relative)
                    }
                }
                .ToDictionary(x => x.Id)
            );

        var sut = new CustomerBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.Empty], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task LoadBatchAsync_Should_Return_Empty_Collection_When_No_Matches_Found()
    {
        var dataRepository = Substitute.For<IDataRepository<ICustomer>>();

        dataRepository
            .GetBatch(Arg.Any<IReadOnlyList<Guid>>(), Arg.Any<Func<ICustomer, Customer>>(), Arg.Any<CancellationToken>())
            .Returns(new Dictionary<Guid, Customer>());

        var sut = new CustomerBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

        var result = await sut.LoadAsync([Guid.Empty], CancellationToken.None);

        result.Should().NotBeEmpty();
        result.MatchSnapshot();
    }
}
