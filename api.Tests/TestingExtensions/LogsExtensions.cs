using Microsoft.Extensions.Logging;

namespace api.Tests.TestingExtensions;

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

    public static TestingLogMessage[] GetLogsResults<T>(this ILogger<T> logger) =>
        logger.GetLogsResultsCollection().ToArray();

    public static TestingLogMessage[] GetLogsResults<T>(this ILogger<T> logger, LogLevel targetLogLevel) =>
        logger.GetLogsResultsCollection().Where(x => x.LogLevel == targetLogLevel).ToArray();
}
