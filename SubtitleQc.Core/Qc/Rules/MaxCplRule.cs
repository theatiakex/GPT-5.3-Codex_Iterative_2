using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MaxCplRule : IQcRule
{
    private readonly int _threshold;

    public MaxCplRule(int threshold)
    {
        _threshold = threshold;
    }

    public string Name => "MaxCpl";

    public IReadOnlyList<QcIssue> Evaluate(Cue cue, QcContext context)
    {
        int longestLine = cue.Lines.DefaultIfEmpty(string.Empty).Max(line => line.Length);
        if (longestLine <= _threshold)
        {
            return Array.Empty<QcIssue>();
        }

        string message = $"Longest line has {longestLine} chars; limit is {_threshold}.";
        return new[] { new QcIssue(Name, message) };
    }
}
