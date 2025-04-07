using System.Net.Mail;
using ui.Extensions;
using ui.Models;

namespace ui.Tests.Extensions;

public class SmtpExtensionsTests
{
    [Theory]
    [InlineData(null, null, true)]
    [InlineData(null, "", true)]
    [InlineData("", null, true)]
    [InlineData("", "", true)]
    [InlineData("username", "password", false)]
    public void UseDefaultCredentials_Should_Return_Expected_When_Credentials_Match_Expectation(
        string? username,
        string? password,
        bool expected
    )
    {
        var config = new SmtpConfig
        {
            Username = username,
            Password = password
        };

        var result = config.UseDefaultCredentials();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null, null, false)]
    [InlineData(null, "", false)]
    [InlineData("", null, false)]
    [InlineData("", "", false)]
    [InlineData("username", "password", true)]
    public void GetNetworkCredentials_Should_Return_Expected_When_Credentials_Match_Expectation(
        string? username,
        string? password,
        bool expected
    )
    {
        var config = new SmtpConfig
        {
            Username = username,
            Password = password
        };

        var result = config.GetNetworkCredentials() is not null;

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetSmtpClient_Should_Return_Client()
    {
        var config = new SmtpConfig();

        var result = config.GetSmtpClient();

        Assert.IsType<SmtpClient>(result);
    }
}
