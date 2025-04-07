using api.GraphExtensions.TypeExtensions;
using api.Shared.Types.Interfaces;

namespace api.GraphExtensions.Tests.TypeExtensions;

public class IBaseIdTypeExtensionTests
{
    [Fact]
    public void InternalId_Should_Return_Id()
    {
        var item = new MockItem(Guid.NewGuid());
        var sut = new IBaseIdTypeExtension();

        var result = sut.GetInternalId(item);

        Assert.Equivalent(item.Id, result, true);
    }

    private record MockItem(Guid Id) : IBaseId;
}
