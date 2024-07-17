namespace api.Shared.Extensions.Tests;

public class ExtensionsTests
{
    public record MockConfig
    {
        public string? Name { get; set; }
    }

    [Fact]
    public void Adding_Bound_Options_Should_Register_In_Dependency_Injection_Using_Section_Name()
    {
        var config = new Dictionary<string, object>
        {
            ["test"] = new
            {
                Name = "Hello"
            }
        }
        .ToConfiguration();
        
        var serviceProvider = new ServiceCollection()
            .AddBoundOptions<MockConfig>(config, "test")
            .Services
            .BuildServiceProvider();

        var result = serviceProvider.GetService<IOptions<MockConfig>>();

        result.Should().NotBeNull();
        result!.Value.Name.Should().Be("Hello");
    }

    [Fact]
    public void Adding_Bound_Options_Should_Register_In_Dependency_Injection_Using_Class_Name()
    {
        var config = new Dictionary<string, object>
        {
            [nameof(MockConfig)] = new
            {
                Name = "Hello"
            }
        }
        .ToConfiguration();
        
        var serviceProvider = new ServiceCollection()
            .AddBoundOptions<MockConfig>(config)
            .Services
            .BuildServiceProvider();

        var result = serviceProvider.GetService<IOptions<MockConfig>>();

        result.Should().NotBeNull();
        result!.Value.Name.Should().Be("Hello");
    }
}
