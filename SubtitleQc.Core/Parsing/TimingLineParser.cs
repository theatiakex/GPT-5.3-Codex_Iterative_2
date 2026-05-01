using System.Text.RegularExpressions;

namespace SubtitleQc.Core.Parsing;

internal static partial class TimingLineParser
{
    [GeneratedRegex(@"^\s*(?<start>\S+)\s*-->\s*(?<end>\S+)")]
    private static partial Regex TimingRegex();

    public static (TimeSpan start, TimeSpan end) Parse(string timingLine)
    {
        Match match = TimingRegex().Match(timingLine);
        if (!match.Success)
        {
            throw new FormatException($"Invalid timing line: '{timingLine}'.");
        }

        TimeSpan start = TimestampParser.Parse(match.Groups["start"].Value);
        TimeSpan end = TimestampParser.Parse(match.Groups["end"].Value);
        return (start, end);
    }
}
