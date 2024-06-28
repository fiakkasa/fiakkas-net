using Microsoft.Extensions.Logging;

namespace api.Tests.TestingModels;

public record TestingLogMessage(
    LogLevel LogLevel,
    string? OriginalMessage,
    KeyValuePair<string, object?>[]? Arguments,
    string? ExceptionType = default,
    string? ExceptionMessage = default,
    string? ExceptionSource = default
);
