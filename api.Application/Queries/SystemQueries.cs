using api.Application.Models;

namespace api.Application.Queries;

[QueryType]
public sealed class SystemQueries
{
    public SystemInfoItem GetSystemStatus([Service] SystemInfoItem systemInfoItem) => systemInfoItem;
}
