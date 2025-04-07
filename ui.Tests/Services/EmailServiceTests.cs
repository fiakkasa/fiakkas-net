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
    private const string _defaultSenderAddress = "email@user.com";
    private const string _senderAddress = "sender@email.com";
    private const string _recipientAddress = "recipient@email.com";
    private const string _subject = "Subject";
    private const string _body = "Body\r\nHello";
    private const string _bodyHtml = "Body<br />Hello";

    private static readonly EmailConfig _config = new()
    {
        AlwaysUseDefaultSenderAddress = true,
        DefaultSenderAddress = _defaultSenderAddress,
        DefaultRecipientAddress = "recipient@user.com",
        PlainTextSignature = "Hello!",
        HtmlSignature = "<br />Hello!"
    };

    private readonly ILogger<EmailService> _logger = Substitute.For<ILogger<EmailService>>();
    private readonly IOptionsSnapshot<EmailConfig> _optionsSnapshot = Substitute.For<IOptionsSnapshot<EmailConfig>>();
    private readonly IHtmlParser _parser = HtmlExtensions.ParserFactory();
    private readonly ISmtpService _smtpService = Substitute.For<ISmtpService>();

    private EmailService GetEmailService(EmailConfig? config = default)
    {
        if (config is not null)
        {
            _optionsSnapshot.Value.Returns(config);
        }

        return new(_smtpService, _parser, _optionsSnapshot, _logger);
    }

    [Theory]
    [InlineData(true, _defaultSenderAddress)]
    [InlineData(false, _senderAddress)]
    public async Task Send_Should_Send_Email_With_Sender_Address_Based_On_AlwaysUseDefaultSenderAddress_Value(
        bool alwaysUseDefaultSenderAddress,
        string expected
    )
    {
        var service = GetEmailService(_config with
        {
            AlwaysUseDefaultSenderAddress = alwaysUseDefaultSenderAddress
        });

        var result = await service.Send(_senderAddress, _recipientAddress, _subject, _body);
        var sendCall =
            _smtpService
                .ReceivedCalls()
                .Select(x => x.GetOriginalArguments())
                .FirstOrDefault()
                ?.OfType<MailMessage>()
                .FirstOrDefault();

        Assert.True(result.IsT0);
        Assert.NotNull(sendCall);
        Assert.Equal(expected, sendCall.From?.Address);
        Assert.Equal(1, sendCall.To?.Count);
        Assert.Equal(_recipientAddress, sendCall.To?[0].Address);
        Assert.EndsWith(_subject, sendCall.Subject);

        Assert.Equal(2, sendCall.AlternateViews.Count);

        Assert.Equal("text/html; charset=utf-8", sendCall.AlternateViews[0].ContentType.ToString());
        await using var htmlStream = sendCall.AlternateViews[0].ContentStream;
        var htmlString = await new StreamReader(htmlStream).ReadToEndAsync();
        Assert.Equal(_bodyHtml + _config.HtmlSignature, htmlString);

        Assert.Equal("text/plain; charset=utf-8", sendCall.AlternateViews[1].ContentType.ToString());
        await using var plaintTextStream = sendCall.AlternateViews[1].ContentStream;
        var plaintTextString = await new StreamReader(plaintTextStream).ReadToEndAsync();
        Assert.Equal(_body + _config.PlainTextSignature, plaintTextString);
    }

    [Theory]
    [InlineData(
        default,
        default,
        default,
        default,
        nameof(EmailErrorCodeType.Required),
        nameof(EmailErrorCodeType.Required),
        nameof(EmailErrorCodeType.Required),
        nameof(EmailErrorCodeType.Required)
    )]
    [InlineData(
        "",
        "",
        "",
        "",
        nameof(EmailErrorCodeType.Required),
        nameof(EmailErrorCodeType.Required),
        nameof(EmailErrorCodeType.Required),
        nameof(EmailErrorCodeType.Required)
    )]
    [InlineData(
        " ",
        " ",
        " ",
        " ",
        nameof(EmailErrorCodeType.Required),
        nameof(EmailErrorCodeType.Required),
        nameof(EmailErrorCodeType.Required),
        nameof(EmailErrorCodeType.Required)
    )]
    [InlineData(
        "hello",
        "",
        "<div>Hello</div>",
        "<div <span",
        nameof(EmailErrorCodeType.InvalidEmailAddress),
        nameof(EmailErrorCodeType.Required),
        nameof(EmailErrorCodeType.MarkupIsNotAllowed),
        nameof(EmailErrorCodeType.UnusableContent)
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
        var resultErrorMessages =
            result
                .AsT1
                .Select(x => x.ErrorMessage)
                .ToArray();

        Assert.True(result.IsT1);
        Assert.Equal(expectedErrors.Length, result.AsT1.Count);
        Assert.All(
            expectedErrors,
            (x, i) => Assert.Equal(x, resultErrorMessages[i])
        );
    }

    [Fact]
    public async Task Send_Should_Not_Send_Email_When_Error_Occurs()
    {
#pragma warning disable CA2012
        _smtpService
            .Send(Arg.Any<MailMessage>(), Arg.Any<CancellationToken>())
            .Throws(new Exception("Splash!"));
#pragma warning restore CA2012
        var service = GetEmailService(_config);

        var result = await service.Send(_senderAddress, _recipientAddress, _subject, _body);
        var errorLogs = _logger.GetLogsResults(LogLevel.Error);

        Assert.True(result.IsT2);
        Assert.IsType<InvalidOperationException>(result.AsT2);
        Assert.Equal(nameof(EmailErrorCodeType.FailedToSend), result.AsT2.Message);
        Assert.Single(errorLogs);
        Assert.Equal("Splash!", errorLogs[0].ExceptionMessage);
    }

    [Fact]
    public async Task SendFrom_Should_Send_Email_With_Default_Recipient_Address()
    {
        var service = GetEmailService(_config);

        var result = await service.SendFrom(_senderAddress, _subject, _body);

        Assert.True(result.IsT0);
    }

    [Fact]
    public async Task SendFrom_Should_Not_Send_Email_When_Error_Occurs()
    {
        var service = GetEmailService();
        _optionsSnapshot.Value.Throws(new Exception("Splash!"));

        var result = await service.SendFrom(_senderAddress, _subject, _body);
        var errorLogs = _logger.GetLogsResults(LogLevel.Error);

        Assert.True(result.IsT2);
        Assert.IsType<InvalidOperationException>(result.AsT2);
        Assert.Equal(nameof(EmailErrorCodeType.FailedToSend), result.AsT2.Message);
        Assert.Single(errorLogs);
        Assert.Equal("Splash!", errorLogs[0].ExceptionMessage);
    }

    [Fact]
    public async Task SendTo_Should_Send_Email_To_Default_Sender_Address()
    {
        var service = GetEmailService(_config);

        var result = await service.SendTo(_recipientAddress, _subject, _body);

        Assert.True(result.IsT0);
    }

    [Fact]
    public async Task SendTo_Should_Not_Send_Email_When_Error_Occurs()
    {
        var service = GetEmailService();
        _optionsSnapshot.Value.Throws(new Exception("Splash!"));

        var result = await service.SendTo(_recipientAddress, _subject, _body);
        var errorLogs = _logger.GetLogsResults(LogLevel.Error);

        Assert.True(result.IsT2);
        Assert.IsType<InvalidOperationException>(result.AsT2);
        Assert.Equal(nameof(EmailErrorCodeType.FailedToSend), result.AsT2.Message);
        Assert.Single(errorLogs);
        Assert.Equal("Splash!", errorLogs[0].ExceptionMessage);
    }
}
