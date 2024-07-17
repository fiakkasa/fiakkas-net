using api.Customers.Interfaces;
using api.Customers.Models;

namespace api.Customers.Mappers.Tests;

public class CustomerMappersTests
{
    public record CustomerMockEntity : ICustomer
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? UpdatedAt { get; init; }
        public long Version { get; init; }
        public string Title { get; init; } = string.Empty;
        public Uri? Href { get; init; }
    }

    [Fact]
    public void Map_Should_Return_Data()
    {
        var item = new CustomerMockEntity
        {
            Id = new Guid("18e483e4-6961-4b25-88a9-d1d0a5161109"),
            CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedAt = null,
            Version = 1,
            Title = "Title",
            Href = new Uri("/test", UriKind.Relative)
        };

        var result = item.Map();

        result.Should().BeOfType<Customer>();
        result.MatchSnapshot();
    }
}
