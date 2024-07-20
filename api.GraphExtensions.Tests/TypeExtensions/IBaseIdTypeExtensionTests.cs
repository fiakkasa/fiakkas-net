using api.Shared.Types.Interfaces;

namespace api.GraphExtensions.TypeExtensions.Tests;

public class IBaseIdTypeExtensionTests
{
    public record MockItem(Guid Id) : IBaseId { }

    [Fact]
    public void InternalId_Should_Return_Id()
    {
        var item = new MockItem(Guid.NewGuid());
        var sut = new IBaseIdTypeExtension();

        var result = sut.GetInternalId(item);
        result.Should().Be(item.Id);
    }
}
