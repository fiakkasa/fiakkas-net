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

    public static IConfiguration ToConfiguration(this string config) =>
        new ConfigurationBuilder()
            .AddJsonStream(
                new MemoryStream(
                    Encoding.UTF8.GetBytes(config)
                )
            )
            .Build();

    public static T AddToConfigurationBuilder<T>(this T builder, Dictionary<string, object> config) where T : IConfigurationBuilder
    {
        builder.AddJsonStream(
            new MemoryStream(
                JsonSerializer.SerializeToUtf8Bytes(config)
            )
        );

        return builder;
    }

    public static T AddToConfigurationBuilder<T>(this T builder, string config) where T : IConfigurationBuilder
    {
        builder.AddJsonStream(
            new MemoryStream(
                Encoding.UTF8.GetBytes(config)
            )
        );

        return builder;
    }
}
