using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Qc.Abstractions;

public interface IQcRule
{
    string Name { get; }

    IReadOnlyList<QcIssue> Evaluate(Cue cue, QcContext context);
}
