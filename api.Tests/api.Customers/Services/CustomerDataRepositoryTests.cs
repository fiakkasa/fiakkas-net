using api.Customers.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api.Customers.Services.Tests;

public class CustomerDataRepositoryTests
{
    [Fact]
    public void ResolveSet_Should_Return_Data()
    {
        var item = new CustomerEntity
        {
            Id = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109"),
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

        result.Should().ContainSingle();
        result.MatchSnapshot();
    }
}
