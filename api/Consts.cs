namespace api;

[ExcludeFromCodeCoverage]
public static class Consts
{
    public const string HealthEndPoint = "/health";
    public const string GraphQLEndPoint = "/graphql";
    public const string GraphQLSchemaVisualizerEndPoint = "/voyager";

    public const string DataFile = "data.json";
    public const string DataFileSectionPath = "data";

    public const string ApiHealthName = "API";
    public const string GraphQLHealthName = "Graph";

    public const int GraphQLMaxExecutionDepthRule = 8;
    public const int GraphQLPagingMaxPageSize = 1_000;
    public const int GraphQLPagingDefaultPageSize = 100;
    public const bool GraphQLPagingIncludeTotalCount = true;

    public const string LogPropertyAppVersion = "AppVersion";
}
