using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace api.Tests.TestingExtensions;

public static class ConfigExtensions
{
    public static T AddToConfigBuilder<T>(this T builder, Dictionary<string, object> config) where T : IConfigurationBuilder
    {
        builder.AddJsonStream(
            new MemoryStream(
                JsonSerializer.SerializeToUtf8Bytes(config)
            )
        );

        return builder;
    }
}
