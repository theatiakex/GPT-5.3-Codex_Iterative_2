using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class OverlapCheckRule : IQcRule
{
    public string Name => "OverlapCheck";

    public IReadOnlyList<QcIssue> Evaluate(Cue cue, QcContext context)
    {
        if (!OverlapsAnyPreviousCue(cue, context.Cues))
        {
            return Array.Empty<QcIssue>();
        }

        return new[] { new QcIssue(Name, "Cue overlaps a previously started cue.") };
    }

    private static bool OverlapsAnyPreviousCue(Cue cue, IReadOnlyList<Cue> allCues)
    {
        return allCues
            .Where(existing => existing.Id != cue.Id && existing.Start <= cue.Start)
            .Any(existing => existing.End > cue.Start);
    }
}
