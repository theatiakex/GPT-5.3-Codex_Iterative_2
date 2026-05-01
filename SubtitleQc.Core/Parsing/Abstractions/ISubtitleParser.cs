using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Parsing.Abstractions;

public interface ISubtitleParser
{
    SubtitleDocument Parse(string rawContent);
}
