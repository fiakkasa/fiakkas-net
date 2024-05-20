using api.Application.Extensions;
using api.Customers.Extensions;
using api.GraphExtensions.Extensions;
using api.Portfolio.Extensions;
using api.Technologies.Extensions;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using HotChocolate.Execution.Options;

namespace api.Extensions;

public static class GraphQLExtensions
{
    public static IRequestExecutorBuilder AddApiGraphQL(this IServiceCollection services, bool isDev) =>
        services
            .AddGraphQLServer()
            .InitializeOnStartup()
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
            .AddQueryType()
            .AddApiApplication()
            .AddApiCustomers()
            .AddApiPortfolio()
            .AddApiTechnologies()
            .AddApiGraphExtensions()
            .TrimTypes();

    public static IEndpointRouteBuilder MapApiGraphQL(this IEndpointRouteBuilder app, bool isDev)
    {
        if (isDev)
            app.MapGraphQLVoyager(Consts.GraphQLSchemaVisualizerEndPoint);

        app
          .MapGraphQL(Consts.GraphQLEndPoint)
          .WithOptions(new()
          {
              Tool = { Enable = isDev },
              EnableSchemaRequests = isDev,
              EnableGetRequests = false,
              AllowedGetOperations = AllowedGetOperations.Query
          });

        return app;
    }
}
