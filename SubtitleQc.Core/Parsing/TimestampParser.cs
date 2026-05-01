namespace SubtitleQc.Core.Parsing;

internal static class TimestampParser
{
    public static TimeSpan Parse(string rawValue)
    {
        string normalized = rawValue.Trim().Replace(',', '.');
        string[] formats = ["hh\\:mm\\:ss\\.fff", "mm\\:ss\\.fff", "hh\\:mm\\:ss\\.ff"];
        foreach (string format in formats)
        {
            if (TimeSpan.TryParseExact(normalized, format, null, out TimeSpan value))
            {
                return value;
            }
        }

        throw new FormatException($"Invalid timestamp: '{rawValue}'.");
    }
}
