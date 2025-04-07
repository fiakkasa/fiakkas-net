using app.Shared.Proxy.Models;

namespace app.Shared.Proxy.Mappers;

public static class ForwardedHeadersConfigMappers
{
    public static ForwardedHeadersOptions ToModel(this ForwardedHeadersConfig config)
    {
        var model = new ForwardedHeadersOptions
        {
            ForwardedForHeaderName = config.ForwardedForHeaderName,
            ForwardedHostHeaderName = config.ForwardedHostHeaderName,
            ForwardedProtoHeaderName = config.ForwardedProtoHeaderName,
            ForwardedPrefixHeaderName = config.ForwardedPrefixHeaderName,
            OriginalForHeaderName = config.OriginalForHeaderName,
            OriginalHostHeaderName = config.OriginalHostHeaderName,
            OriginalProtoHeaderName = config.OriginalProtoHeaderName,
            OriginalPrefixHeaderName = config.OriginalPrefixHeaderName,
            ForwardedHeaders = config.ForwardedHeaders,
            ForwardLimit = config.ForwardLimit,
            AllowedHosts = config.AllowedHosts,
            RequireHeaderSymmetry = config.RequireHeaderSymmetry
        };

        model.KnownProxies.Clear();
        foreach (var item in config.KnownProxies)
        {
            model.KnownProxies.Add(item);
        }

        model.KnownNetworks.Clear();
        foreach (var item in config.KnownNetworks)
        {
            model.KnownNetworks.Add(item);
        }

        return model;
    }
}
