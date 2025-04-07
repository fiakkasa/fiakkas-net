using ui.Extensions;

namespace ui.Tests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("HELLO_WORLD", "Hello world")]
    [InlineData("hello_world", "Hello world")]
    [InlineData("HELLO-WORLD", "Hello world")]
    [InlineData("hello-world", "Hello world")]
    [InlineData("hello world", "Hello world")]
    [InlineData("helloWorld", "Hello world")]
    [InlineData("HelloWorld", "Hello world")]
    [InlineData("Hello World!", "Hello world")]
    [InlineData(" helloWorld_TEST-abc XyZ ", "Helloworld test abc xyz")]
    public void ToPrettyErrorMessage_Should_Return_Transformed_Message_When_Length_Is_Greater_Than_Zero(
        string? message,
        string expected
    )
    {
        var result = message.ToPrettyErrorMessage();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToPrettyErrorMessageOfException_Should_Return_Transformed_Message()
    {
        var exception = new Exception("SPLASH!");

        var result = exception.ToPrettyErrorMessage();

        Assert.Equal("Splash", result);
    }

    [Fact]
    public void ToPrettyErrorMessageOfValidationResult_Should_Return_Transformed_Message()
    {
        var exception = new ValidationResult("SPLASH!");

        var result = exception.ToPrettyErrorMessage();

        Assert.Equal("Splash", result);
    }
}
