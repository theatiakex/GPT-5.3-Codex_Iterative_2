using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class EmptyCueCheckRule : IQcRule
{
    public string Name => "EmptyCueCheck";

    public IReadOnlyList<QcIssue> Evaluate(Cue cue, QcContext context)
    {
        if (cue.Lines.Any(line => !string.IsNullOrWhiteSpace(line)))
        {
            return Array.Empty<QcIssue>();
        }

        return new[] { new QcIssue(Name, "Cue contains no visible text.") };
    }
}
