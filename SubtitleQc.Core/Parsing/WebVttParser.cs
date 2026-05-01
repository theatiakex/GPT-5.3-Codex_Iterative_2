using System.Text.RegularExpressions;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Parsing.Abstractions;

namespace SubtitleQc.Core.Parsing;

public sealed class WebVttParser : ISubtitleParser
{
    public SubtitleDocument Parse(string rawContent)
    {
        string normalized = Normalize(rawContent);
        string body = RemoveHeader(normalized);
        string[] blocks = SplitBlocks(body);
        var cues = blocks.Select(ParseBlock).Where(cue => cue is not null).Cast<Cue>().ToArray();
        return new SubtitleDocument(cues);
    }

    private static string Normalize(string rawContent)
    {
        return (rawContent ?? string.Empty).Replace("\r\n", "\n").Trim();
    }

    private static string RemoveHeader(string content)
    {
        if (!content.StartsWith("WEBVTT", StringComparison.OrdinalIgnoreCase))
        {
            return content;
        }

        int splitIndex = content.IndexOf("\n\n", StringComparison.Ordinal);
        return splitIndex < 0 ? string.Empty : content[(splitIndex + 2)..];
    }

    private static string[] SplitBlocks(string body)
    {
        return Regex.Split(body, @"\n\s*\n").Where(static block => block.Length > 0).ToArray();
    }

    private static Cue? ParseBlock(string block)
    {
        string[] lines = block.Split('\n');
        int timingLineIndex = FindTimingLineIndex(lines);
        if (timingLineIndex < 0)
        {
            return null;
        }

        (TimeSpan start, TimeSpan end) = TimingLineParser.Parse(lines[timingLineIndex]);
        string[] textLines = lines.Skip(timingLineIndex + 1).ToArray();
        return new Cue(Guid.NewGuid().ToString("N"), start, end, textLines);
    }

    private static int FindTimingLineIndex(IReadOnlyList<string> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Contains("-->", StringComparison.Ordinal))
            {
                return i;
            }
        }

        return -1;
    }

}
