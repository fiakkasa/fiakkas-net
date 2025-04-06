namespace api.Extensions;

public static class GraphQLExtensions
{
    public static IServiceCollection AddApiGraphQL(this IServiceCollection services, bool isDev = false)
    {
        services
            .AddApiGraphQLServer(isDev)
            .AddApiGraphQLEndpoints();

        return services;
    }

    public static IRequestExecutorBuilder AddApiGraphQLEndpoints(this IRequestExecutorBuilder builder) =>
        builder
            .AddApiAchievements()
            .AddApiApplication()
            .AddApiCategories()
            .AddApiContactItems()
            .AddApiCustomers()
            .AddApiEducationItems()
            .AddApiLanguages()
            .AddApiPortfolio()
            .AddApiTextItems()
            .AddApiGraphExtensions();

    public static IRequestExecutorBuilder AddApiGraphQLServer(this IServiceCollection services, bool isDev) =>
        services
            .AddGraphQLServer()
            .InitializeOnStartup()
            .AddMaxExecutionDepthRule(Consts.GraphQLMaxExecutionDepthRule, isDev)
            .DisableIntrospection(!isDev)
            .AddFiltering()
            .AddSorting()
            .ModifyOptions(options =>
                options.StripLeadingIFromInterface = Consts.GraphQLOptionsStripLeadingIFromInterface
            )
            .ModifyRequestOptions(options => options.IncludeExceptionDetails = isDev)
            .ModifyCostOptions(costOptions =>
                costOptions.EnforceCostLimits = Consts.GraphQLCostEnforceCostLimits
            )
            .ModifyPagingOptions(pagingOptions =>
            {
                pagingOptions.MaxPageSize = Consts.GraphQLPagingMaxPageSize;
                pagingOptions.DefaultPageSize = Consts.GraphQLPagingDefaultPageSize;
                pagingOptions.IncludeTotalCount = Consts.GraphQLPagingIncludeTotalCount;
            })
            .AddGlobalObjectIdentification()
            .AddQueryType()
            .TrimTypes();

    public static IEndpointRouteBuilder MapApiGraphQL(this IEndpointRouteBuilder app, bool isDev)
    {
        if (isDev)
        {
            app.MapGraphQLVoyager(Consts.GraphQLSchemaVisualizerEndPoint);
        }

        app
            .MapGraphQL(Consts.GraphQLEndPoint)
            .WithOptions(new()
            {
                Tool =
                {
                    Enable = isDev,
                    DisableTelemetry = true
                },
                EnableSchemaRequests = isDev,
                EnableGetRequests = false,
                AllowedGetOperations = AllowedGetOperations.Query
            });

        return app;
    }
}
