using AngleSharp.Html.Parser;
using Snapshooter.Xunit;
using ui.Enums;
using ui.Extensions;
using ui.Interfaces;
using ui.Models;

namespace ui.Tests.Extensions;

public class EmailExtensionsTests
{
    private readonly IHtmlParser _parser = HtmlExtensions.ParserFactory();

    [Fact]
    public void AddEmailService_Should_Add_Services_And_Validated_Options()
    {
        var expectedSmtpConfig = new SmtpConfig
        {
            Host = "smtp-host",
            Port = 25,
            EnableSsl = false,
            Username = "username",
            Password = "password"
        };
        var expectedEmailConfig = new EmailConfig
        {
            AlwaysUseDefaultSenderAddress = true,
            DefaultSenderAddress = "email@user.com",
            DefaultRecipientAddress = "email@user.com",
            PlainTextSignature = "Hello!",
            HtmlSignature = "Hello!"
        };
        var serviceProvider =
            new ServiceCollection()
                .AddSingleton(
                    new Dictionary<string, object>
                    {
                        [nameof(SmtpConfig)] = expectedSmtpConfig,
                        [nameof(EmailConfig)] = expectedEmailConfig
                    }.ToConfiguration()
                )
                .AddSingleton(Substitute.For<IHtmlParser>())
                .AddLogging()
                .AddEmailService()
                .BuildServiceProvider();

        var smtpConfigResult = serviceProvider.GetRequiredService<IOptionsSnapshot<SmtpConfig>>();
        var emailConfigResult = serviceProvider.GetRequiredService<IOptionsSnapshot<EmailConfig>>();
        var smtpServiceResult = serviceProvider.GetService<ISmtpService>();
        var emailServiceResult = serviceProvider.GetService<IEmailService>();

        Assert.Equivalent(expectedSmtpConfig, smtpConfigResult.Value, true);
        Assert.Equivalent(expectedEmailConfig, emailConfigResult.Value, true);
        Assert.NotNull(smtpServiceResult);
        Assert.NotNull(emailServiceResult);

        new
        {
            smtpConfigResult,
            emailConfigResult
        }.MatchSnapshot();
    }

    [Theory]
    [InlineData("user@email.com")]
    [InlineData(null, nameof(EmailErrorCodeType.Required))]
    [InlineData("", nameof(EmailErrorCodeType.Required))]
    [InlineData(" ", nameof(EmailErrorCodeType.Required))]
    [InlineData("hello-world", nameof(EmailErrorCodeType.InvalidEmailAddress))]
    public void ValidateEmailAddress_Should_Yield_Results_When_Conditions_Met(
        string? emailAddress,
        string? expected = default
    )
    {
        var result = emailAddress.ValidateEmailAddress("member").ToArray();

        Assert.Equal(expected, result.FirstOrDefault()?.ErrorMessage);
    }

    [Theory]
    [InlineData("Hello!")]
    [InlineData(null, nameof(EmailErrorCodeType.Required))]
    [InlineData("", nameof(EmailErrorCodeType.Required))]
    [InlineData(" ", nameof(EmailErrorCodeType.Required))]
    [InlineData("<div></div>", nameof(EmailErrorCodeType.MarkupIsNotAllowed))]
    [InlineData("<meta /> Test", nameof(EmailErrorCodeType.MarkupIsNotAllowed))]
    [InlineData("<div>Hello!</div>", nameof(EmailErrorCodeType.MarkupIsNotAllowed))]
    [InlineData("<html <div  ", nameof(EmailErrorCodeType.UnusableContent))]
    public async Task ValidateEmailContent_Should_Yield_Results_When_Conditions_Met(
        string? content,
        string? expected = default
    )
    {
        var result = await content.ValidateEmailContent(_parser, "member");

        Assert.Equal(expected, result.FirstOrDefault()?.ErrorMessage);
    }

    [Theory]
    [InlineData("user@email.com", true, "default@email.com", "default@email.com")]
    [InlineData("user@email.com", false, "default@email.com", "user@email.com")]
    public void GetSenderMailAddress_Should_Return_Expected_Address(
        string senderAddress,
        bool useDefaultSenderAddress,
        string defaultSenderAddress,
        string expected
    )
    {
        var result = senderAddress.GetSenderMailAddress(useDefaultSenderAddress, defaultSenderAddress);

        Assert.Equal(expected, result.Address);
    }

    [Theory]
    [InlineData("Test", true, "user@email.com", "On Behalf of <user@email.com> | Test")]
    [InlineData("Test", false, "user@email.com", "Test")]
    public void GetSubject_Should_Return_Expected_Subject(
        string subject,
        bool isBehalfOf,
        string senderAddress,
        string expected
    )
    {
        var result = subject.GetSubject(isBehalfOf, senderAddress);

        Assert.Equal(expected, result);
    }
}
