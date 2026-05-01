using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MaxLinesRule : IQcRule
{
    private readonly int _threshold;

    public MaxLinesRule(int threshold)
    {
        _threshold = threshold;
    }

    public string Name => "MaxLines";

    public IReadOnlyList<QcIssue> Evaluate(Cue cue, QcContext context)
    {
        if (cue.Lines.Count <= _threshold)
        {
            return Array.Empty<QcIssue>();
        }

        string message = $"Cue has {cue.Lines.Count} lines; limit is {_threshold}.";
        return new[] { new QcIssue(Name, message) };
    }
}
