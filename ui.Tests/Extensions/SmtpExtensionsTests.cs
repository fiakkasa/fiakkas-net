using System.Net.Mail;
using ui.Models;

namespace ui.Extensions.Tests;

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

        result.Should().Be(expected);
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

        var result = config.GetNetworkCredentials() is { };

        result.Should().Be(expected);
    }

    [Fact]
    public void GetSmtpClient_Should_Return_Client()
    {
        var config = new SmtpConfig();

        var result = config.GetSmtpClient();

        result.Should().BeOfType<SmtpClient>();
    }
}
