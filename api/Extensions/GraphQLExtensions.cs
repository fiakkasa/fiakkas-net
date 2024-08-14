using api.Achievements.Extensions;
using api.Application.Extensions;
using api.Categories.Extensions;
using api.ContactItems.Extensions;
using api.Customers.Extensions;
using api.EducationItems.Extensions;
using api.GraphExtensions.Extensions;
using api.Languages.Extensions;
using api.Portfolio.Extensions;
using api.TextItems.Extensions;

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
            .AllowIntrospection(isDev)
            .AddApolloTracing(isDev switch
            {
                true => TracingPreference.OnDemand,
                _ => TracingPreference.Never
            })
            .AddFiltering()
            .AddSorting()
            .SetPagingOptions(new()
            {
                MaxPageSize = Consts.GraphQLPagingMaxPageSize,
                DefaultPageSize = Consts.GraphQLPagingDefaultPageSize,
                IncludeTotalCount = Consts.GraphQLPagingIncludeTotalCount
            })
            .ModifyOptions(options => options.StripLeadingIFromInterface = true)
            .ModifyRequestOptions(options => options.IncludeExceptionDetails = isDev)
            .AddGlobalObjectIdentification()
            .AddQueryType()
            .TrimTypes();

    public static IEndpointRouteBuilder MapApiGraphQL(this IEndpointRouteBuilder app, bool isDev)
    {
        if (isDev)
            app.MapGraphQLVoyager(Consts.GraphQLSchemaVisualizerEndPoint);

        app
            .MapGraphQL(Consts.GraphQLEndPoint)
            .WithOptions(new()
            {
                Tool =
                {
                    Enable = isDev
                },
                EnableSchemaRequests = isDev,
                EnableGetRequests = false,
                AllowedGetOperations = AllowedGetOperations.Query
            });

        return app;
    }
}
