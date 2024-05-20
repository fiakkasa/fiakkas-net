using api.Application.Models;

namespace api.Application.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public sealed class SystemQueries
{
    public SystemInfoItem GetSystemStatus([Service] SystemInfoItem systemInfoItem) => systemInfoItem;
}
