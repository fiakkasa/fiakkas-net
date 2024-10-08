namespace ui.Tests.TestingModels;

[ExcludeFromCodeCoverage]
public record TestingLogMessage(
    LogLevel LogLevel,
    string? OriginalMessage,
    KeyValuePair<string, object?>[]? Arguments,
    string? ExceptionType = default,
    string? ExceptionMessage = default,
    string? ExceptionSource = default
);
