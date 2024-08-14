using AngleSharp.Html.Parser;
using ui.Enums;
using ui.Interfaces;
using ui.Models;

namespace ui.Extensions.Tests;

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
                        [nameof(EmailConfig)] = expectedEmailConfig,
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

        smtpConfigResult.Value.Should().Be(expectedSmtpConfig);
        emailConfigResult.Value.Should().Be(expectedEmailConfig);
        smtpServiceResult.Should().NotBeNull();
        emailServiceResult.Should().NotBeNull();
    }

    [Theory]
    [InlineData("user@email.com")]
    [InlineData(null, nameof(EmailErrorCodeType.Required))]
    [InlineData("", nameof(EmailErrorCodeType.Required))]
    [InlineData(" ", nameof(EmailErrorCodeType.Required))]
    [InlineData("hello-world", nameof(EmailErrorCodeType.InvalidEmailAddress))]
    public void ValidateEmailAddress_Should_Yield_Results_When_Conditions_Met(
        string? emailAddress,
        params string[] expectedErrors
    )
    {
        var result = emailAddress.ValidateEmailAddress("member").ToArray();

        result.Should().HaveCount(expectedErrors.Length);
        expectedErrors.Should().ContainInOrder(result.Select(x => x.ErrorMessage));
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
        params string[] expectedErrors
    )
    {
        var result = await content.ValidateEmailContent(_parser, "member");

        result.Should().HaveCount(expectedErrors.Length);

        result.Should().HaveCount(expectedErrors.Length);
        expectedErrors.Should().ContainInOrder(result.Select(x => x.ErrorMessage));
    }

    [Theory]
    [InlineData("user@email.com", true, "default@email.com", "default@email.com")]
    [InlineData("user@email.com", false, "default@email.com", "user@email.com")]
    public void GetSenderMailAddress_Should_Return_Expected_Address(
        string senderAddress,
        bool useDefaultSenderAddress,
        string defaultSenderAddress,
        string expectedAddress
    )
    {
        var result = senderAddress.GetSenderMailAddress(useDefaultSenderAddress, defaultSenderAddress);

        result.Address.Should().Be(expectedAddress);
    }

    [Theory]
    [InlineData("Test", true, "user@email.com", "On Behalf of <user@email.com> | Test")]
    [InlineData("Test", false, "user@email.com", "Test")]
    public void GetSubject_Should_Return_Expected_Subject(
        string subject,
        bool isBehalfOf,
        string senderAddress,
        string expectedSubject
    )
    {
        var result = subject.GetSubject(isBehalfOf, senderAddress);

        result.Should().Be(expectedSubject);
    }
}
