using Polly;
using Polly.Retry;
using StrawberryShake;
using ui.GraphQL;

namespace ui.Extensions;

public static class FiakkasNetApiExtensions
{
    public static IServiceCollection AddFiakkasNetApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatedOptions<FiakkasNetApiConfig>();

        var fiakkasNetApiConfig = configuration.GetConfiguration<FiakkasNetApiConfig>();

        var waitAndRetryOptionsPolicy = new RetryStrategyOptions
        {
            ShouldHandle =
                new PredicateBuilder()
                    .Handle<ArgumentNullException>()
                    .Handle<GraphQLClientException>(),
            BackoffType = fiakkasNetApiConfig.DelayBackoffType,
            UseJitter = fiakkasNetApiConfig.UseJitter,
            MaxRetryAttempts = fiakkasNetApiConfig.MaxRetryAttempts,
            Delay = fiakkasNetApiConfig.Delay
        };
        services.AddResiliencePipeline(
            nameof(FiakkasNetApi),
            builder => builder.AddRetry(waitAndRetryOptionsPolicy)
        );
        services
            .AddFiakkasNetApi()
            .ConfigureHttpClient(
                (serviceProvider, client) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptionsMonitor<FiakkasNetApiConfig>>().CurrentValue;
                    client.BaseAddress = config.BaseUrl;
                }
            );

        return services;
    }
}
