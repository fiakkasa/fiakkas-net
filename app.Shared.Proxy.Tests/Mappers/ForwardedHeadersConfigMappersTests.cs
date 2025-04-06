using app.Shared.Proxy.Mappers;
using System.Net;

namespace app.Shared.Proxy.Tests.Mappers;

public class ForwardedHeadersConfigMappersTests
{
    [Fact]
    public void ToModel_Should_Map_ForwardedHeadersConfig_To_ForwardedHeadersOptions()
    {
        var config = new ForwardedHeadersConfig
        {
            Enable = true,
            ForwardedForHeaderName = "XYZ-Forwarded-For",
            ForwardedHostHeaderName = "XYZ-Forwarded-Host",
            ForwardedProtoHeaderName = "XYZ-Forwarded-Proto",
            ForwardedPrefixHeaderName = "XYZ-Forwarded-Prefix",
            OriginalForHeaderName = "XYZ-Original-For",
            OriginalHostHeaderName = "XYZ-Original-Host",
            OriginalProtoHeaderName = "XYZ-Original-Proto",
            OriginalPrefixHeaderName = "XYZ-Original-Prefix",
            ForwardedHeaders = ForwardedHeaders.All,
            ForwardLimit = 10,
            AllowedHosts = ["example.com", "test.com"],
            RequireHeaderSymmetry = true
        };

        config.KnownProxies.Clear();
        config.KnownProxies.Add(IPAddress.IPv6Loopback);
        config.KnownProxies.Add(IPAddress.Parse("192.168.1.1"));

        config.KnownNetworks.Clear();
        config.KnownNetworks.Add(new(IPAddress.Loopback, 8));
        config.KnownNetworks.Add(new(IPAddress.Parse("192.168.1.1"), 8));

        var model = config.ToModel();

        Assert.Equal(config.ForwardedForHeaderName, model.ForwardedForHeaderName);
        Assert.Equal(config.ForwardedHostHeaderName, model.ForwardedHostHeaderName);
        Assert.Equal(config.ForwardedProtoHeaderName, model.ForwardedProtoHeaderName);
        Assert.Equal(config.ForwardedPrefixHeaderName, model.ForwardedPrefixHeaderName);
        Assert.Equal(config.OriginalForHeaderName, model.OriginalForHeaderName);
        Assert.Equal(config.OriginalHostHeaderName, model.OriginalHostHeaderName);
        Assert.Equal(config.OriginalProtoHeaderName, model.OriginalProtoHeaderName);
        Assert.Equal(config.OriginalPrefixHeaderName, model.OriginalPrefixHeaderName);
        Assert.Equal(config.ForwardedHeaders, model.ForwardedHeaders);
        Assert.Equal(config.ForwardLimit, model.ForwardLimit);
        Assert.Equal(config.AllowedHosts, model.AllowedHosts);
        Assert.Equal(config.RequireHeaderSymmetry, model.RequireHeaderSymmetry);
        Assert.Equal(config.KnownProxies.Count, model.KnownProxies.Count);
        Assert.Equal(config.KnownNetworks.Count, model.KnownNetworks.Count);
        Assert.All(config.KnownProxies, (item, i) => model.KnownProxies[i].Equals(item));
        Assert.All(config.KnownNetworks, (item, i) => model.KnownNetworks[i].Equals(item));
    }
}
