using api.Customers.Interfaces;
using api.Customers.Models;
using api.Shared.Interfaces;

namespace api.Customers.Queries;

public class CustomerQueriesTests
{
    [Fact]
    public void GetCustomers_Should_Return_Data()
    {
        var item = new Customer
        {
            Id = Guid.Empty,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };
        var service = Substitute.For<IDataRepository<ICustomer>>();
        service
            .Get(Arg.Any<Func<ICustomer, Customer>>())
            .Returns(new[] { item }.AsQueryable());
        var qut = new CustomerQueries();

        var result = qut.GetCustomers(service);

        result.Should().NotBeEmpty();
        result.Should().BeAssignableTo<IQueryable<Customer>>();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
