using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MinDurationRule : IQcRule
{
    private readonly TimeSpan _threshold;

    public MinDurationRule(TimeSpan threshold)
    {
        _threshold = threshold;
    }

    public string Name => "MinDuration";

    public IReadOnlyList<QcIssue> Evaluate(Cue cue, QcContext context)
    {
        if (cue.Duration >= _threshold)
        {
            return Array.Empty<QcIssue>();
        }

        string message = $"Cue duration is {cue.Duration.TotalSeconds:F3}s; minimum is {_threshold.TotalSeconds:F3}s.";
        return new[] { new QcIssue(Name, message) };
    }
}
