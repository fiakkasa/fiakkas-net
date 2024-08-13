using AngleSharp.Html.Parser;
using ui.Enums;
using ui.Interfaces;
using ui.Models;

namespace ui.Extensions.Tests;

public class EmailExtensionsTests
{
    private readonly HtmlParser _parser = new(new HtmlParserOptions
    {
        IsScripting = false,
        SkipComments = true,
        SkipRCDataText = true,
        SkipCDATA = true,
        SkipScriptText = true
    });

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
    [InlineData("", nameof(EmailErrorCodeType.REQUIRED), nameof(EmailErrorCodeType.INVALID_EMAIL_ADDRESS))]
    [InlineData("hello-world", nameof(EmailErrorCodeType.INVALID_EMAIL_ADDRESS))]
    public void ValidateEmailAddress_Should_Yield_Results_When_Conditions_Met(
        string emailAddress,
        params string[] expectedErrors
    )
    {
        var result = emailAddress.ValidateEmailAddress("member").ToArray();

        result.Should().HaveCount(expectedErrors.Length);

        if (result.Length > 0)
            result.Select(x => x.ErrorMessage).Should().IntersectWith(expectedErrors);
    }

    [Theory]
    [InlineData("Hello!")]
    [InlineData("<html <div  ", nameof(EmailErrorCodeType.UNUSABLE_CONTENT))]
    [InlineData("<div>Hello!</div>", nameof(EmailErrorCodeType.MARKUP_IS_NOT_ALLOWED))]
    [InlineData("<meta /><div>Hello!</div>", nameof(EmailErrorCodeType.MARKUP_IS_NOT_ALLOWED))]
    [InlineData("<meta />", nameof(EmailErrorCodeType.UNUSABLE_CONTENT), nameof(EmailErrorCodeType.MARKUP_IS_NOT_ALLOWED))]
    public async Task ValidateEmailPlainTextContent_Should_Yield_Results_When_Conditions_Met(
        string content,
        params string[] expectedErrors
    )
    {
        using var document = await _parser.ParseDocumentAsync(content);

        var result = document.ValidateEmailPlainTextContent("member").ToArray();

        result.Should().HaveCount(expectedErrors.Length);

        if (result.Length > 0)
            result.Select(x => x.ErrorMessage).Should().IntersectWith(expectedErrors);
    }

    [Theory]
    [InlineData("<div>Hello!</div>")]
    [InlineData("<html <div  ", nameof(EmailErrorCodeType.UNUSABLE_CONTENT))]
    [InlineData("<div class='test'></div>", nameof(EmailErrorCodeType.UNUSABLE_CONTENT), nameof(EmailErrorCodeType.HTML_ATTRIBUTES_ARE_NOT_ALLOWED))]
    [InlineData("<div class='test'>Hello!</div>", nameof(EmailErrorCodeType.HTML_ATTRIBUTES_ARE_NOT_ALLOWED))]
    [InlineData("<script></script>", nameof(EmailErrorCodeType.UNUSABLE_CONTENT), nameof(EmailErrorCodeType.SCRIPT_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<script></script> Hello!", nameof(EmailErrorCodeType.SCRIPT_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<style></style>", nameof(EmailErrorCodeType.UNUSABLE_CONTENT), nameof(EmailErrorCodeType.STYLE_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<style></style> Hello!", nameof(EmailErrorCodeType.STYLE_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<link />", nameof(EmailErrorCodeType.UNUSABLE_CONTENT), nameof(EmailErrorCodeType.LINK_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<link /> Hello!", nameof(EmailErrorCodeType.LINK_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<meta />", nameof(EmailErrorCodeType.UNUSABLE_CONTENT), nameof(EmailErrorCodeType.META_TAGS_ARE_NOT_ALLOWED))]
    [InlineData("<meta /> Hello!", nameof(EmailErrorCodeType.META_TAGS_ARE_NOT_ALLOWED))]
    public async Task ValidateEmailHtmlContent_Should_Yield_Results_When_Conditions_Met(
        string content,
        params string[] expectedErrors
    )
    {
        using var document = await _parser.ParseDocumentAsync(content);

        var result = document.ValidateEmailHtmlContent("member").ToArray();

        result.Should().HaveCount(expectedErrors.Length);

        if (result.Length > 0)
            result.Select(x => x.ErrorMessage).Should().IntersectWith(expectedErrors);
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
