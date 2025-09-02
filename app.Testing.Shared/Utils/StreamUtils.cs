namespace app.Testing.Shared.Utils;

[ExcludeFromCodeCoverage]
public static class StreamUtils
{
    public static string StreamToString(Stream stream)
    {
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
