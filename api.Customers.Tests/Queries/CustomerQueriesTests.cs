using api.Customers.DataLoaders;
using api.Customers.Interfaces;
using api.Customers.Models;
using api.Customers.Queries;
using GreenDonut;

namespace api.Customers.Tests.Queries;

public class CustomerQueriesTests
{
    [Fact]
    public void GetCustomers_Should_Return_Data()
    {
        var item = new Customer
        {
            Id = new("18e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new("/test", UriKind.Relative)
        };
        var dataRepository = new MockDataRepository<ICustomer>([item]);

        var result = CustomerQueries.GetCustomers(dataRepository);

        Assert.Single(result);
        Assert.IsAssignableFrom<IQueryable<Customer>>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetCustomerById_Should_Return_Data_When_Found()
    {
        var id = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109");
        var item = new Customer
        {
            Id = id,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new("/test", UriKind.Relative)
        };
        var dataRepository = new MockDataRepository<ICustomer>([item]);
        var dataLoader = new CustomerBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await CustomerQueries.GetCustomerById(
            id,
            dataLoader,
            default
        );

        Assert.NotNull(result);
        Assert.IsType<Customer>(result);
        result.MatchSnapshot();
    }

    [Fact]
    public async Task GetCustomerById_Should_Return_Null_When_Not_Found()
    {
        var id = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109");
        var dataRepository = new MockDataRepository<ICustomer>([]);
        var dataLoader = new CustomerBatchDataLoader(
            dataRepository,
            AutoBatchScheduler.Default,
            new()
        );

        var result = await CustomerQueries.GetCustomerById(
            id,
            dataLoader,
            default
        );

        Assert.Null(result);
    }
}
