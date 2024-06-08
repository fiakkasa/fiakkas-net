using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace api.Tests.TestingExtensions;

public static class ConfigExtensions
{
    public static IConfigurationRoot GetConfigRoot(this Dictionary<string, object> config) =>
        new ConfigurationBuilder()
            .AddJsonStream(
                new MemoryStream(
                    JsonSerializer.SerializeToUtf8Bytes(config)
                )
            )
            .Build();

    public static T AddToConfigBuilder<T>(this T builder, string config) where T : IConfigurationBuilder
    {
        builder.AddJsonStream(
            new MemoryStream(
                Encoding.UTF8.GetBytes(config)
            )
        );

        return builder;
    }
}
