using System.Text.RegularExpressions;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Parsing.Abstractions;

namespace SubtitleQc.Core.Parsing;

public sealed class SrtParser : ISubtitleParser
{
    public SubtitleDocument Parse(string rawContent)
    {
        string normalized = Normalize(rawContent);
        string[] blocks = SplitBlocks(normalized);
        var cues = blocks.Select(ParseBlock).Where(cue => cue is not null).Cast<Cue>().ToArray();
        return new SubtitleDocument(cues);
    }

    private static string Normalize(string rawContent)
    {
        return (rawContent ?? string.Empty).Replace("\r\n", "\n").Trim();
    }

    private static string[] SplitBlocks(string content)
    {
        return Regex.Split(content, @"\n\s*\n").Where(static block => block.Length > 0).ToArray();
    }

    private static Cue? ParseBlock(string block)
    {
        string[] lines = block.Split('\n');
        int timingLineIndex = GetTimingLineIndex(lines);
        if (timingLineIndex < 0)
        {
            return null;
        }

        (TimeSpan start, TimeSpan end) = TimingLineParser.Parse(lines[timingLineIndex]);
        string[] textLines = lines.Skip(timingLineIndex + 1).ToArray();
        return new Cue(Guid.NewGuid().ToString("N"), start, end, textLines);
    }

    private static int GetTimingLineIndex(IReadOnlyList<string> lines)
    {
        if (lines.Count > 0 && lines[0].Contains("-->", StringComparison.Ordinal))
        {
            return 0;
        }

        return lines.Count > 1 ? 1 : -1;
    }

}
