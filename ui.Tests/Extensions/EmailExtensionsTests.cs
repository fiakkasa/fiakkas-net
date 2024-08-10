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

    [Fact]
    public async Task ValidateEmailContent_Should_Yield_No_Results_When_Valid()
    {
        var content = "<div>Hello!</div>";

        var result = await content.ValidateEmailContent(_parser, "member").ToListAsync();

        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("   ", 1, nameof(EmailErrorCodeType.EMPTY_CONTENT))]
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
        string expectedFirst,
        string? expectedSecond = default
    )
    {
        var result = await content.ValidateEmailContent(_parser, "member").ToListAsync();

        result.Should().HaveCount(expectedCount);
        result[0].ErrorMessage.Should().Be(expectedFirst);

        if (expectedSecond is { })
            result[1].ErrorMessage.Should().Be(expectedSecond);
    }
}
