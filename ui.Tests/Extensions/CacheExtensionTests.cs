using LazyCache;
using ui.Extensions;

namespace ui.Tests.Extensions;

public class CacheExtensionTests
{
    [Fact]
    public void AddUiCache_Should_Add_Cache()
    {
        var serviceProvider =
            new ServiceCollection()
                .AddUiCache()
                .BuildServiceProvider();

        var result = serviceProvider.GetRequiredService<IAppCache>();

        result.Should().NotBeNull();
    }
}
