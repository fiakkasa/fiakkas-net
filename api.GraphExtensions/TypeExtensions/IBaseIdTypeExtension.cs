namespace api.GraphExtensions.TypeExtensions;

[ExtendObjectType<IBaseId>]
public sealed class IBaseIdTypeExtension
{
    public Guid GetInternalId([Parent] IBaseId parent) => parent.Id;
}
