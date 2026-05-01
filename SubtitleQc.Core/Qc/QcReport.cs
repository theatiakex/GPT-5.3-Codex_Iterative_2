namespace SubtitleQc.Core.Qc;

public sealed record QcReport(IReadOnlyList<QcResult> Results);
