namespace SubtitleQc.Core.Qc;

public sealed record QcResult(
    string CueId,
    QcStatus Status,
    IReadOnlyList<QcIssue> Issues);
