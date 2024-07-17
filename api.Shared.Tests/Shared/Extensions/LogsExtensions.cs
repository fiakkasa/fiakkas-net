using api.Tests.Shared.Models;

namespace api.Tests.Shared.Extensions;

public static class LogsExtensions
{
    public static IEnumerable<TestingLogMessage> GetLogsResultsCollection<T>(this ILogger<T> logger) =>
        logger
            .ReceivedCalls()
            .Select(x =>
                x.GetOriginalArguments() switch
                {
                    [
                        LogLevel logLevel,
                        _,
                        IEnumerable<KeyValuePair<string, object?>> arguments,
                        Exception exception,
                        ..
                    ] =>
                        new TestingLogMessage(
                            LogLevel: logLevel,
                            OriginalMessage: arguments.First(x => x.Key == "{OriginalFormat}").Value?.ToString(),
                            Arguments: arguments.ToArray(),
                            ExceptionType: exception.GetType().Name,
                            ExceptionMessage: exception.Message,
                            ExceptionSource: exception.Source
                        ),
                    [
                        LogLevel logLevel,
                        _,
                        IEnumerable<KeyValuePair<string, object?>> arguments,
                        ..
                    ] =>
                        new TestingLogMessage(
                            LogLevel: logLevel,
                            OriginalMessage: arguments.First(x => x.Key == "{OriginalFormat}").Value?.ToString(),
                            Arguments: arguments.ToArray()
                        ),
                    _ => null
                }
            )
            .OfType<TestingLogMessage>();
}
