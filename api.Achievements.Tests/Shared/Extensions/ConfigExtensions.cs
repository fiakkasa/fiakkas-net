namespace api.Tests.Shared.Extensions;

public static class ConfigExtensions
{
    public static IConfiguration ToConfiguration(this Dictionary<string, object> config) =>
        new ConfigurationBuilder()
            .AddJsonStream(
                new MemoryStream(
                    JsonSerializer.SerializeToUtf8Bytes(config)
                )
            )
            .Build();
}
