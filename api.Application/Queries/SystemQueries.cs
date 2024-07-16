using api.Application.Models;

namespace api.Application.Queries;

[QueryType]
public static class SystemQueries
{
    public static SystemInfoItem GetSystemStatus([Service] SystemInfoItem systemInfoItem) => systemInfoItem;
}
