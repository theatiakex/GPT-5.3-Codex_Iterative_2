namespace SubtitleQc.Core.Models;

public sealed record ExternalQcData(
    IReadOnlyList<TimeSpan> ShotChangeTimestamps,
    IReadOnlyList<int> ShotChangeFrames,
    double? FrameRate);
