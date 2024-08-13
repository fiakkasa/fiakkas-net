using AngleSharp.Html.Parser;
using NSubstitute.ExceptionExtensions;
using System.Net.Mail;
using System.Threading;
using ui.Enums;
using ui.Extensions;
using ui.Interfaces;
using ui.Models;
using ui.Services;

namespace ui.Tests.Services;

public class EmailServiceTests
{
    private readonly ISmtpService _smtpService;
    private readonly IHtmlParser _parser = HtmlExtensions.ParserFactory();
    private readonly IOptionsSnapshot<EmailConfig> _optionsSnapshot;
    private readonly ILogger<EmailService> _logger;

    private const string _defaultSenderAddress = "email@user.com";
    private const string _senderAddress = "sender@email.com";
    private const string _recipientAddress = "recipient@email.com";
    private const string _subject = "Subject";
    private const string _body = "Body";
    private static readonly EmailConfig _config = new()
    {
        AlwaysUseDefaultSenderAddress = true,
        DefaultSenderAddress = _defaultSenderAddress,
        DefaultRecipientAddress = "recipient@user.com",
        PlainTextSignature = "Hello!",
        HtmlSignature = "Hello!"
    };

    public EmailServiceTests()
    {
        _smtpService = Substitute.For<ISmtpService>();
        _optionsSnapshot = Substitute.For<IOptionsSnapshot<EmailConfig>>();
        _logger = Substitute.For<ILogger<EmailService>>();
    }

    private EmailService GetEmailService(EmailConfig? config = default)
    {
        if (config is { })
            _optionsSnapshot.Value.Returns(config);

        return new EmailService(_smtpService, _parser, _optionsSnapshot, _logger);
    }

    [Theory]
    [InlineData(true, _defaultSenderAddress)]
    [InlineData(false, _senderAddress)]
    public async Task Send_Should_Send_Email_With_Sender_Address_Based_On_AlwaysUseDefaultSenderAddress_Value(
        bool alwaysUseDefaultSenderAddress,
        string expectedSenderAddress
    )
    {
        var service = GetEmailService(_config with { AlwaysUseDefaultSenderAddress = alwaysUseDefaultSenderAddress });

        var result = await service.Send(_senderAddress, _recipientAddress, _subject, _body);
        var sendCall =
            _smtpService.ReceivedCalls()
                .Select(x => x.GetOriginalArguments())
                .FirstOrDefault()
                ?.OfType<MailMessage>()
                .FirstOrDefault();

        result.IsT0.Should().BeTrue();
        sendCall.Should().NotBeNull();
        sendCall!.From!.Address.Should().Be(expectedSenderAddress);
        sendCall.To!.Count.Should().Be(1);
        sendCall.To[0]!.Should().Be(_recipientAddress);
        sendCall.Subject.Should().EndWith(_subject);
        sendCall.Body.Should().Be(_body + _config.HtmlSignature);
        sendCall.AlternateViews.Should().ContainSingle();
        sendCall.AlternateViews[0].ContentType.Should().Be("text/plain; charset=utf-8");
        using var plaintTextStream = sendCall.AlternateViews[0].ContentStream;
        var plaintTextString = await new StreamReader(plaintTextStream).ReadToEndAsync();
        plaintTextString.Should().Be(_body + _config.PlainTextSignature);
    }

    [Theory]
    [InlineData(
        default,
        default,
        default,
        default,
        nameof(EmailErrorCodeType.REQUIRED),
        nameof(EmailErrorCodeType.REQUIRED),
        nameof(EmailErrorCodeType.REQUIRED),
        nameof(EmailErrorCodeType.REQUIRED)
    )]
    [InlineData(
        "",
        "",
        "",
        "",
        nameof(EmailErrorCodeType.REQUIRED),
        nameof(EmailErrorCodeType.REQUIRED),
        nameof(EmailErrorCodeType.REQUIRED),
        nameof(EmailErrorCodeType.REQUIRED)
    )]
    [InlineData(
        " ",
        " ",
        " ",
        " ",
        nameof(EmailErrorCodeType.REQUIRED),
        nameof(EmailErrorCodeType.REQUIRED),
        nameof(EmailErrorCodeType.REQUIRED),
        nameof(EmailErrorCodeType.REQUIRED)
    )]
    [InlineData(
        "hello",
        "",
        "<div>Hello</div>",
        "<div <span",
        nameof(EmailErrorCodeType.INVALID_EMAIL_ADDRESS),
        nameof(EmailErrorCodeType.REQUIRED),
        nameof(EmailErrorCodeType.MARKUP_IS_NOT_ALLOWED),
        nameof(EmailErrorCodeType.UNUSABLE_CONTENT)
    )]
    public async Task Send_Should_Not_Send_Email_When_Validation_Errors_Found(
        string? senderAddress,
        string? recipientAddress,
        string? subject,
        string? body,
        params string[] expectedErrors
    )
    {
        var service = GetEmailService(_config);

        var result = await service.Send(
            senderAddress!,
            recipientAddress!,
            subject!,
            body!
        );

        result.IsT1.Should().BeTrue();
        result.AsT1.Should().HaveCount(expectedErrors.Length);
        expectedErrors.Should().ContainInOrder(result.AsT1.Select(x => x.ErrorMessage));
    }

    [Fact]
    public async Task Send_Should_Not_Send_Email_When_Error_Occurs()
    {
        _smtpService
            .Send(Arg.Any<MailMessage>(), Arg.Any<CancellationToken>())
            .Throws(new Exception("Splash!"));
        var service = GetEmailService(_config);

        var result = await service.Send(_senderAddress, _recipientAddress, _subject, _body);
        var errorLogs = _logger.GetLogsResults(targetLogLevel: LogLevel.Error);

        result.IsT2.Should().BeTrue();
        result.AsT2.Should().BeOfType<InvalidOperationException>();
        result.AsT2.Message.Should().Be(nameof(EmailErrorCodeType.FAILED_TO_SEND));
        errorLogs.Should().ContainSingle();
        errorLogs[0].ExceptionMessage.Should().Be("Splash!");
    }

    [Fact]
    public async Task SendFrom_Should_Send_Email_With_Default_Recipient_Address()
    {
        var service = GetEmailService(_config);

        var result = await service.SendFrom(_senderAddress, _subject, _body);

        result.IsT0.Should().BeTrue();
    }

    [Fact]
    public async Task SendFrom_Should_Not_Send_Email_When_Error_Occurs()
    {
        var service = GetEmailService();
        _optionsSnapshot.Value.Throws(new Exception("Splash!"));

        var result = await service.SendFrom(_senderAddress, _subject, _body);
        var errorLogs = _logger.GetLogsResults(targetLogLevel: LogLevel.Error);

        result.IsT2.Should().BeTrue();
        result.AsT2.Should().BeOfType<InvalidOperationException>();
        result.AsT2.Message.Should().Be(nameof(EmailErrorCodeType.FAILED_TO_SEND));
        errorLogs.Should().ContainSingle();
        errorLogs[0].ExceptionMessage.Should().Be("Splash!");
    }

    [Fact]
    public async Task SendTo_Should_Send_Email_To_Default_Sender_Address()
    {
        var service = GetEmailService(_config);

        var result = await service.SendTo(_recipientAddress, _subject, _body);

        result.IsT0.Should().BeTrue();
    }

    [Fact]
    public async Task SendTo_Should_Not_Send_Email_When_Error_Occurs()
    {
        var service = GetEmailService();
        _optionsSnapshot.Value.Throws(new Exception("Splash!"));

        var result = await service.SendTo(_recipientAddress, _subject, _body);
        var errorLogs = _logger.GetLogsResults(targetLogLevel: LogLevel.Error);

        result.IsT2.Should().BeTrue();
        result.AsT2.Should().BeOfType<InvalidOperationException>();
        result.AsT2.Message.Should().Be(nameof(EmailErrorCodeType.FAILED_TO_SEND));
        errorLogs.Should().ContainSingle();
        errorLogs[0].ExceptionMessage.Should().Be("Splash!");
    }
}
