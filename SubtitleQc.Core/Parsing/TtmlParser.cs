using System.Xml.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Parsing.Abstractions;

namespace SubtitleQc.Core.Parsing;

public sealed class TtmlParser : ISubtitleParser
{
    public SubtitleDocument Parse(string rawContent)
    {
        if (string.IsNullOrWhiteSpace(rawContent))
        {
            return new SubtitleDocument(Array.Empty<Cue>());
        }

        XDocument xml = XDocument.Parse(rawContent);
        var cues = ReadCueElements(xml).Select(CreateCue).ToArray();
        return new SubtitleDocument(cues);
    }

    private static IEnumerable<XElement> ReadCueElements(XDocument xml)
    {
        return xml.Descendants().Where(element => element.Name.LocalName == "p");
    }

    private static Cue CreateCue(XElement cueElement)
    {
        TimeSpan start = ReadTimeAttribute(cueElement, "begin");
        TimeSpan end = ReadTimeAttribute(cueElement, "end");
        string[] lines = ReadLines(cueElement).ToArray();
        return new Cue(Guid.NewGuid().ToString("N"), start, end, lines);
    }

    private static TimeSpan ReadTimeAttribute(XElement cueElement, string attributeName)
    {
        string? rawValue = cueElement.Attribute(attributeName)?.Value;
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            throw new FormatException($"TTML cue is missing '{attributeName}'.");
        }

        return TimestampParser.Parse(rawValue);
    }

    private static IEnumerable<string> ReadLines(XElement cueElement)
    {
        string flattened = string.Concat(cueElement.Nodes().Select(ExtractNodeText));
        return NormalizeLines(flattened).Split('\n');
    }

    private static string ExtractNodeText(XNode node)
    {
        if (node is XElement element && element.Name.LocalName == "br")
        {
            return "\n";
        }

        return node is XText textNode ? textNode.Value : node.ToString(SaveOptions.DisableFormatting);
    }

    private static string NormalizeLines(string text)
    {
        return text.Replace("\r\n", "\n").Replace('\r', '\n');
    }
}
