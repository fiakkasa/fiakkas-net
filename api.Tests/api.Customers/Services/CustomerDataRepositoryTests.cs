using api.Customers.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.Customers.Services;

public class CustomerDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Customers()
    {
        var item = new CustomerEntity
        {
            Id = Guid.Empty,
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };
        var configData = new CustomersDataConfig
        {
            Customers = [item]
        };
        var configOptions = Substitute.For<IOptionsSnapshot<CustomersDataConfig>>();
        configOptions.Value.Returns(configData);

        var sut = new CustomerDataRepository(Substitute.For<ILogger<CustomerDataRepository>>(), configOptions);

        var result = sut.Get();

        result.Should().NotBeEmpty();
        result.First().Should().BeEquivalentTo(item);
        result.MatchSnapshot();
    }
}
