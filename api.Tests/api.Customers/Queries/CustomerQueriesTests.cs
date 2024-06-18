using api.Customers.Interfaces;
using api.Customers.Models;

namespace api.Customers.Queries.Tests;

public class CustomerQueriesTests
{
    [Fact]
    public void GetCustomers_Should_Return_Data()
    {
        var item = new Customer
        {
            Id = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };
        var dataRepository = new MockDataRepository<ICustomer>([item]);
        var sut = new CustomerQueries();

        var result = sut.GetCustomers(dataRepository);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<Customer>>();
        result.MatchSnapshot();
    }
}
