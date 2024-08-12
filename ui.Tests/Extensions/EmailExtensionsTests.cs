using AngleSharp.Html.Parser;
using ui.Enums;
using ui.Interfaces;
using ui.Models;

namespace ui.Extensions.Tests;

public class EmailExtensionsTests
{
    private readonly HtmlParser _parser = new();

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
            DefaultSenderAddress = "email@user.com",
            DefaultRecipientAddress = "email@user.com",
            DefaultPlainTextSignature = "Hello!",
            DefaultHtmlSignature = "Hello!"
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
    [InlineData("user@email.com", 0)]
    [InlineData("", 2, nameof(EmailErrorCodeType.REQUIRED), nameof(EmailErrorCodeType.INVALID_EMAIL_ADDRESS))]
    [InlineData("hello-world", 1, nameof(EmailErrorCodeType.INVALID_EMAIL_ADDRESS))]
    public void ValidateEmailAddress_Should_Yield_Results_When_Conditions_Met(
        string emailAddress,
        int expectedCount,
        string? expectedFirst = default,
        string? expectedSecond = default
    )
    {
        var result = emailAddress.ValidateEmailAddress("member").ToArray();

        result.Should().HaveCount(expectedCount);

        if (expectedFirst is { })
            result[0].ErrorMessage.Should().Be(expectedFirst);

        if (expectedSecond is { })
            result[1].ErrorMessage.Should().Be(expectedSecond);
    }

    [Theory]
    [InlineData("<div>Hello!</div>", 0)]
    [InlineData("<html <div  ", 1, nameof(EmailErrorCodeType.UNUSABLE_CONTENT))]
    [InlineData("<div class='test'></div>", 2, nameof(EmailErrorCodeType.UNUSABLE_CONTENT), nameof(EmailErrorCodeType.HTML_ATTRIBUTES_ARE_NOT_ALLOWED))]
    [InlineData("<div class='test'>Hello!</div>", 1, nameof(EmailErrorCodeType.HTML_ATTRIBUTES_ARE_NOT_ALLOWED))]
    [InlineData("<script></script>", 2, nameof(EmailErrorCodeType.UNUSABLE_CONTENT), nameof(EmailErrorCodeType.SCRIPT_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<script></script> Hello!", 1, nameof(EmailErrorCodeType.SCRIPT_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<style></style>", 2, nameof(EmailErrorCodeType.UNUSABLE_CONTENT), nameof(EmailErrorCodeType.STYLE_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<style></style> Hello!", 1, nameof(EmailErrorCodeType.STYLE_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<link />", 2, nameof(EmailErrorCodeType.UNUSABLE_CONTENT), nameof(EmailErrorCodeType.LINK_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<link /> Hello!", 1, nameof(EmailErrorCodeType.LINK_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<meta />", 2, nameof(EmailErrorCodeType.UNUSABLE_CONTENT), nameof(EmailErrorCodeType.META_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<meta /> Hello!", 1, nameof(EmailErrorCodeType.META_TAGS_ARE_NOT_ALLOWED))]
    public async Task ValidateEmailContent_Should_Yield_Results_When_Conditions_Met(
        string content,
        int expectedCount,
        string? expectedFirst = default,
        string? expectedSecond = default
    )
    {
        using var document = await _parser.ParseDocumentAsync(content);

        var result = document.ValidateEmailContent("member").ToList();

        result.Should().HaveCount(expectedCount);

        if (expectedFirst is { })
            result[0].ErrorMessage.Should().Be(expectedFirst);

        if (expectedSecond is { })
            result[1].ErrorMessage.Should().Be(expectedSecond);
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
