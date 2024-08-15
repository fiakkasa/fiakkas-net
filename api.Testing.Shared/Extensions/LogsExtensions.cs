using api.Testing.Shared.Models;

namespace api.Testing.Shared.Extensions;

[ExcludeFromCodeCoverage]
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
                    ] when arguments.ToArray() is { } arrayArgs => new(
                        logLevel,
                        arrayArgs.FirstOrDefault(item => item.Key == "{OriginalFormat}").Value?.ToString(),
                        arrayArgs,
                        exception.GetType().Name,
                        exception.Message,
                        exception.Source
                    ),
                    [
                        LogLevel logLevel,
                        _,
                        IEnumerable<KeyValuePair<string, object?>> arguments,
                        ..
                    ] when arguments.ToArray() is { } arrayArgs => new TestingLogMessage(
                        logLevel,
                        arrayArgs.FirstOrDefault(item => item.Key == "{OriginalFormat}").Value?.ToString(),
                        arrayArgs
                    ),
                    _ => null
                }
            )
            .OfType<TestingLogMessage>();

    public static TestingLogMessage[] GetLogsResults<T>(this ILogger<T> logger) =>
        logger.GetLogsResultsCollection().ToArray();

    public static TestingLogMessage[] GetLogsResults<T>(this ILogger<T> logger, LogLevel targetLogLevel) =>
        logger.GetLogsResultsCollection().Where(x => x.LogLevel == targetLogLevel).ToArray();
}
